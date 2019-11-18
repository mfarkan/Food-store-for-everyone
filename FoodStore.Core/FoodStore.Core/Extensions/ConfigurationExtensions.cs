using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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
            return config.GetValue<string>("CookieName");
        }
    }
}
