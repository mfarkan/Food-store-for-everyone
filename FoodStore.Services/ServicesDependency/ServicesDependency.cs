using FoodStore.Services.ServiceInterfaces;
using FoodStore.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStore.Services.ServicesDependency
{
    public static class ServicesDependency
    {
        public static void AddNotificationExtensions(this IServiceCollection services)
        {
            services.AddScoped<IMessageSender, MessageSender>();
        }
    }
}
