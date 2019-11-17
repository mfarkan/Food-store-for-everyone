using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStore.Core.MessageOptions
{
    public class MailSettings
    {
        public string MailAddress { get; set; }
        public string MailPassword { get; set; }
        public int MailPort { get; set; }
        public string SmtpServer { get; set; }
        public string SenderName { get; set; }
    }
}


