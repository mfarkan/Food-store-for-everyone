using FoodStore.Core.ServiceInterfaces;
using FoodStore.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStore.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddNotificationExtensions(this IServiceCollection services)
        {
            services.AddScoped<IMessageSender, MessageSender>();
        }
    }
}
