using timely_backend.Models.DTO;

namespace timely_backend.Services {
    public interface IAccountService {
        Task<TokenResponse> Register(UserRegisterModel userRegisterModel);
        Task<TokenResponse> Login(LoginCredentials loginCredentials);
        Task<UserProfile> GetProfile(string email);
        Task<Response> EditProfile(string email, UserProfileEdit userProfileEdit);
        Task<Response> EditPassword(string email, UserPasswordEdit userPasswordEdit);
        Task<Response> Logout(string token);
    }
}