using timely_backend.Models.DTO;

namespace timely_backend.Services {
    public interface IAccountService {
        Task<TokenResponse> register(UserRegisterModel userRegisterModel);
    }
}
