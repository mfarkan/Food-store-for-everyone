using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Core.ServiceInterfaces
{
    public interface IMessageSender
    {
        Task SendEmailAsync(string verificationToken);
        Task SendSmsAsync();
    }
}
