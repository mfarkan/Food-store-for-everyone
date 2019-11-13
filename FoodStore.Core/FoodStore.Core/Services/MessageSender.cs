using FoodStore.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Core.Services
{
    public class MessageSender : IMessageSender
    {
        public Task SendEmailAsync(string verificationToken)
        {
            return Task.FromResult(0);
        }

        public Task SendSmsAsync()
        {
            return Task.FromResult(0);
        }
    }
}
