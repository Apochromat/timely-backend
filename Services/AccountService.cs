using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using timely_backend.Models;
using timely_backend.Models.DTO;

namespace timely_backend.Services {
    public class AccountService : IAccountService {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<AccountService> _logger;
        private readonly ApplicationDbContext _context;

        public AccountService(ILogger<AccountService> logger, ApplicationDbContext context,
            UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager) {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _context = context;
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

            var result = await _userManager.CreateAsync(user, userRegisterModel.Password);
            if (result.Succeeded) {
                _logger.LogInformation("Successful register");
                return new TokenResponse("Successful", "register", null);
                //return await login(new LoginCredentials { Email = userRegisterModel.Email, Password = userRegisterModel.Password });
            }

            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new InvalidOperationException(errors);
        }

        /// <summary>
        /// Log in user
        /// </summary>
        public async Task<TokenResponse> Login(LoginCredentials loginCredentials) {
            var identity = await GetIdentity(loginCredentials.Email.ToLower(), loginCredentials.Password);
            if (identity == null) {
                throw new ArgumentException("Incorrect username or password");
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: JwtConfiguration.Issuer,
                audience: JwtConfiguration.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(JwtConfiguration.Lifetime)),
                signingCredentials: new SigningCredentials(JwtConfiguration.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
            _logger.LogInformation("Successful login");

            return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(jwt),
                identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault(""),
                identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First().Split(","));
        }
        
        /// <summary>
        /// Returns user`s profile
        /// </summary>
        public async Task<UserProfile> GetProfile(string email) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new KeyNotFoundException("User not found");

            user = _userManager.Users.Include( u => u.Roles).ThenInclude(r => r.Role).First(u => u.Id == user.Id);

            return ModelConverter.ToUserProfile(user);
        }

        private async Task<ClaimsIdentity?> GetIdentity(string email, string password) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) return null;

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Email ?? ""),
                new Claim(ClaimTypes.Role, string.Join(",", await _userManager.GetRolesAsync(user)))
            };

            return new ClaimsIdentity(claims, "Token", ClaimTypes.Name, ClaimTypes.Role);
        }
    }
}