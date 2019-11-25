using FoodStore.Core.MessageOptions;
using FoodStore.Core.ServiceInterfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Core.Services
{
    public class MessageSender : IMessageSender
    {
        private readonly MailSettings _settings;
        public MessageSender(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
        {
            var success = false;
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_settings.SenderName, _settings.MailAddress));
            mimeMessage.To.Add(new MailboxAddress(toEmail));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("html")
            {
                Text = message
            };
            try
            {
                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(_settings.SmtpServer, _settings.MailPort, true);
                    await client.AuthenticateAsync(_settings.MailAddress, _settings.MailPassword);
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }

        public Task SendSmsAsync()
        {
            return Task.FromResult(0);
        }
    }
}
