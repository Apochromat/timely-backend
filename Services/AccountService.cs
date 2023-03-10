using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using timely_backend.Models;
using timely_backend.Models.DTO;
using timely_backend.Models.Enum;
using timely_backend.Views;

namespace timely_backend.Services {
    public class AccountService : IAccountService {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<AccountService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IAdminService _adminService;

        public AccountService(ILogger<AccountService> logger, ApplicationDbContext context,
            UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager,
            ICacheService cacheService, IEmailSender emailSender, IConfiguration configuration,
            IAdminService adminService) {
            _cacheService = cacheService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _context = context;
            _emailSender = emailSender;
            _configuration = configuration;
            _adminService = adminService;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        public async Task<TokenResponse> Register(UserRegisterModel userRegisterModel) {
            if (userRegisterModel.Email == null) {
                throw new ArgumentNullException(nameof(userRegisterModel.Email));
            }

            if (userRegisterModel.Password == null) {
                throw new ArgumentNullException(nameof(userRegisterModel.Password));
            }

            if (await _userManager.FindByEmailAsync(userRegisterModel.Email) != null)
                throw new ArgumentException("User with this email already exists");

            User user = ModelConverter.ToUser(userRegisterModel);
            user.AvatarLink = _configuration.GetSection("DefaultUsersConfig")["AvatarLink"];

            var result = await _userManager.CreateAsync(user, userRegisterModel.Password);
            if (result.Succeeded) {
                _logger.LogInformation("Successful register");

                var letter = await SendEmailConfirmationLetter(user.Email);
                var emailDomain = user.Email.Split("@")[1];
                if (_context.Domains.Select(x => x.Url).ToList().Contains(emailDomain)) {
                    await _userManager.AddToRoleAsync(user, ApplicationRoleNames.Teacher); // set role

                    if (_context.Teachers.Where(t => t.Name == user.FullName).IsNullOrEmpty()) {
                        await _adminService.CreateTeacher(
                            new TeacherDTO {
                                Name = user.FullName
                            });
                    }

                    user.Teacher = _context.Teachers.Where(t => t.Name == user.FullName).FirstOrDefault();
                    await _context.SaveChangesAsync();
                }
                else {
                    await _userManager.AddToRoleAsync(user, ApplicationRoleNames.Student);
                }

                return await Login(new LoginCredentials
                    { Email = userRegisterModel.Email, Password = userRegisterModel.Password });
            }

            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new InvalidOperationException(errors);
        }

        /// <summary>
        /// Deletes an user if it exists
        /// </summary>
        public async Task<Response> DeleteUser(string email) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new Response() {
                    Status = "Ok",
                    Message = "There is no user with this email"
                };

            await _userManager.DeleteAsync(user);

            return new Response() {
                Status = "Ok",
                Message = "User was deleted successfully"
            };
        }

        /// <summary>
        /// Send Email confirmation letter
        /// </summary>
        public async Task<Response> SendEmailConfirmationLetter(string email) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new KeyNotFoundException("User not found");
            if (user.EmailConfirmed) throw new InvalidOperationException("Email is already confirmed");

            var config = _configuration.GetSection("EmailConfiguration");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var view = EmailConfirmationView.Page(user.FullName, config.GetValue<string>("SiteURL"), token);

            await _emailSender.SendEmailAsync(user.Email, config.GetValue<string>("ConfirmationTitle"), view);

            _logger.LogInformation("Email confirmation letter successfully sent");
            return new Response() {
                Status = "Ok",
                Message = "Email confirmation letter successfully sent"
            };
        }

        /// <summary>
        /// Confirm user`s email
        /// </summary>
        public async Task<Response> ConfirmEmail(string email, string token) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new KeyNotFoundException("User not found");
            if (user.EmailConfirmed) throw new InvalidOperationException("Email is already confirmed");

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded) {
                _logger.LogInformation("Successful email confirmation");

                return new Response {
                    Status = "Ok",
                    Message = "Successful email confirmation"
                };
            }

            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new InvalidOperationException(errors);
        }

        /// <summary>
        /// Log user in
        /// </summary>
        public async Task<TokenResponse> Login(LoginCredentials loginCredentials) {
            var identity = await GetIdentity(loginCredentials.Email.ToLower(), loginCredentials.Password);
            if (identity == null) {
                throw new ArgumentException("Incorrect username or password");
            }

            var user = _userManager.Users.Where(x => x.Email == loginCredentials.Email).Include(x => x.Group)
                .Include(x => x.Teacher).First();

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: JwtConfiguration.Issuer,
                audience: JwtConfiguration.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(JwtConfiguration.Lifetime)),
                signingCredentials: new SigningCredentials(JwtConfiguration.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            _logger.LogInformation("Successful login");

            return new TokenResponse {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                Email = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault(""),
                Role = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList(),
                IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user),
                Teacher = user.Teacher == null ? null : ModelConverter.ToTeacherDTO(user.Teacher),
                Group = user.Group == null ? null : ModelConverter.ToGroupDTO(user.Group),
                AvatarLink = user.AvatarLink
            };
        }

        /// <summary>
        /// Disable token and log user out
        /// </summary>
        public async Task<Response> Logout(string token) {
            await _cacheService.DisableToken(token);

            _logger.LogInformation("Successful logout");

            return new Response {
                Status = "Ok",
                Message = "Successful logout"
            };
        }

        /// <summary>
        /// Returns user`s profile
        /// </summary>
        public async Task<UserProfile> GetProfile(string email) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new KeyNotFoundException("User not found");

            user = _userManager.Users.Where(u => u.Email == email).Include(u => u.Roles).ThenInclude(r => r.Role)
                .Include(u => u.Teacher).Include(u => u.Group)
                .First();

            _logger.LogInformation("User`s profile was returned successfuly");
            return ModelConverter.ToUserProfile(user);
        }

        /// <summary>
        /// Edit user`s profile
        /// </summary>
        public async Task<Response> EditProfile(string email, UserProfileEdit userProfileEdit) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.FullName = Regex.Replace(userProfileEdit.FullName, @"\s+", " ");
            await _context.SaveChangesAsync();

            _logger.LogInformation("User`s profile was modified successfully");

            return new Response {
                Status = "Ok",
                Message = "Successfully modified"
            };
        }

        /// <summary>
        /// Change user`s password
        /// </summary>
        public async Task<Response> EditPassword(string email, UserPasswordEdit userPasswordEdit) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new KeyNotFoundException("User not found");

            var result = await _userManager.ChangePasswordAsync(user, userPasswordEdit.CurrentPassword,
                userPasswordEdit.NewPassword);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(x => x.Description)));

            _logger.LogInformation("User`s password was changed successfully");

            return new Response {
                Status = "Ok",
                Message = "Password successfully changed"
            };
        }

        /// <summary>
        /// Set user`s group
        /// </summary>
        public async Task<Response> SetGroup(string email, Guid groupId) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new KeyNotFoundException("User not found");
            
            var group = _context.Groups.Where(g => g.Id == groupId).FirstOrDefault();
            if (group == null) throw new KeyNotFoundException("Group not found");

            user = _userManager.Users.Where(u => u.Email == email).Include(u => u.Roles).ThenInclude(r => r.Role)
                .Include(u => u.Teacher).Include(u => u.Group)
                .First();

            user.Group = group;
            await _context.SaveChangesAsync();

            _logger.LogInformation("User`s group was set successfully");

            return new Response {
                Status = "Ok",
                Message = "Group successfully set"
            };
        }

        /// <summary>
        /// Remove user`s group
        /// </summary>
        public async Task<Response> RemoveGroup(string email) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new KeyNotFoundException("User not found");

            user = _userManager.Users.Where(u => u.Email == email).Include(u => u.Roles).ThenInclude(r => r.Role)
                .Include(u => u.Teacher).Include(u => u.Group)
                .First();

            user.Group = null;
            await _context.SaveChangesAsync();

            _logger.LogInformation("User`s group was removed successfully");

            return new Response {
                Status = "Ok",
                Message = "Group successfully removed"
            };
        }
        
        /// <summary>
        /// Set user`s avatar
        /// </summary>
        public async Task<Response> SetAvatar(string email, AvatarLinkDTO model) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new KeyNotFoundException("User not found");
            
            user = _userManager.Users.Where(u => u.Email == email).First();

            user.AvatarLink = model.AvatarLink;
            await _context.SaveChangesAsync();

            _logger.LogInformation("User`s avatar was set successfully");

            return new Response {
                Status = "Ok",
                Message = "Avatar was successfully set"
            };
        }

        /// <summary>
        /// Remove user`s avatar
        /// </summary>
        public async Task<Response> RemoveAvatar(string email) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new KeyNotFoundException("User not found");

            user = _userManager.Users.Where(u => u.Email == email).First();

            user.AvatarLink = _configuration.GetSection("DefaultUsersConfig")["AvatarLink"];
            await _context.SaveChangesAsync();

            _logger.LogInformation("User`s avatar was defaulted successfully");

            return new Response {
                Status = "Ok",
                Message = "Avatar was successfully defaulted"
            };
        }

        private async Task<ClaimsIdentity?> GetIdentity(string email, string password) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) return null;

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Email ?? "")
            };

            foreach (var role in await _userManager.GetRolesAsync(user)) {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return new ClaimsIdentity(claims, "Token", ClaimTypes.Name, ClaimTypes.Role);
        }
    }
}