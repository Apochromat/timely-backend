using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace timely_backend
{
    public class JwtConfiguration
    {
        private static IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

        public static int Lifetime = configuration.GetSection("JwtConfiguration").GetValue<int>("LifetimeMinutes");
        public static string Issuer = configuration.GetSection("JwtConfiguration").GetValue<string>("Issuer"); 
        public static string Audience = configuration.GetSection("JwtConfiguration").GetValue<string>("Audience"); 
        private static string Key = configuration.GetSection("JwtConfiguration").GetValue<string>("Key");

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
