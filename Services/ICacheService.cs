namespace timely_backend.Services {
    public interface ICacheService {
        Task<Boolean> CheckToken(string jwtToken);
        Task DisableToken(string jwtToken);
        Task ClearToken(string jwtToken);
    }
}
