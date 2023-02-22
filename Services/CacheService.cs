using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace timely_backend.Services {
    public class CacheService : ICacheService {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        public DistributedCacheEntryOptions options;
        
        public CacheService(IDistributedCache cache, IConfiguration configuration) {
            _cache = cache;
            _configuration = configuration;
            options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddHours(_configuration.GetSection("JwtConfiguration").GetValue<int>("LogoutAbsoluteExpirationHours")))
                .SetSlidingExpiration(TimeSpan.FromHours(_configuration.GetSection("JwtConfiguration").GetValue<int>("LogoutSlidingExpirationHours")));
        }
        
        public async Task<Boolean> CheckToken(string jwtToken) {
            var env = await _cache.GetAsync(jwtToken);
            if (env == null || String.IsNullOrEmpty(Encoding.UTF8.GetString(env))) {
                return false;
            }
            return true;
        }

        public async Task DisableToken(string jwtToken) {
            var dataToCache = Encoding.UTF8.GetBytes("Disabled");
            await _cache.SetAsync(jwtToken, dataToCache, options);
        }

        public async Task ClearToken(string jwtToken) {
            await _cache.RemoveAsync(jwtToken);
        }
    }
}
