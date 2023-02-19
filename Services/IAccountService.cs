using timely_backend.Models.DTO;

namespace timely_backend.Services {
    public interface IAccountService {
        
        Task<TokenResponse> Register(UserRegisterModel userRegisterModel);
        Task<TokenResponse> Login(LoginCredentials loginCredentials);
    }
}
