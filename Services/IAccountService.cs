using timely_backend.Models.DTO;

namespace timely_backend.Services {
    public interface IAccountService {
        Task<TokenResponse> Register(UserRegisterModel userRegisterModel);
        Task<Response> SendEmailConfirmationLetter(string email);
        Task<Response> ConfirmEmail(string email, string token);
        Task<TokenResponse> Login(LoginCredentials loginCredentials);
        Task<Response> Logout(string token);

        Task<UserProfile> GetProfile(string email);
        Task<Response> EditProfile(string email, UserProfileEdit userProfileEdit);
        Task<Response> EditPassword(string email, UserPasswordEdit userPasswordEdit);
        Task<Response> SetGroup(string email, Guid groupId);
        Task<Response> RemoveGroup(string email);
    }
}