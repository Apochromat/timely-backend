using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using timely_backend.Models;
using timely_backend.Models.DTO;

namespace timely_backend.Services {
    public class AccountService : IAccountService {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountService> _logger;
        private readonly ApplicationDbContext _context;

        public AccountService(ILogger<AccountService> logger, ApplicationDbContext context,
            UserManager<User> userManager, SignInManager<User> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        public async Task<TokenResponse> register(UserRegisterModel userRegisterModel) {
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
    }
}