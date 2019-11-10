using Microsoft.Extensions.Configuration;
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
    }
}
