using Microsoft.Extensions.Configuration;

namespace FoodStore.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetCustomerMaxFailedAccessAttempts(this IConfiguration config)
        {
            return config.GetSection("UserLockConfig")["MaxAttempts"];
        }
        public static string GetCookieName(this IConfiguration config)
        {
            return config["CookieName"];
        }
    }
}
