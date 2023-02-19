using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace timely_backend.Services {
    public class CacheService : ICacheService {
        private readonly IDistributedCache _cache;
        public CacheService(IDistributedCache cache) {
            _cache = cache;
        }

        private static IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddHours(configuration.GetSection("JwtConfiguration").GetValue<int>("LogoutAbsoluteExpirationHours")))
                    .SetSlidingExpiration(TimeSpan.FromHours(configuration.GetSection("JwtConfiguration").GetValue<int>("LogoutSlidingExpirationHours")));

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
