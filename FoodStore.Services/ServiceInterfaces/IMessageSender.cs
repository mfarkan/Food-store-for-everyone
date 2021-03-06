﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Services.ServiceInterfaces
{
    public interface IMessageSender
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string message);
        Task SendSmsAsync();
    }
}
