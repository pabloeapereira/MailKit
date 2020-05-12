using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;

namespace MailKit.Models
{
    public class Email
    {
        public string PrimaryDomain { get; set; }
        public int PrimaryPort { get; set; }
        public string UserNameEmail { get; set; }
        public string UserNamePassword { get; set; }
        public string Subject { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string BccEmail { get; set; }
        public string DisplayName { get; set; }
        public MailPriority Priority { get; set; }
        public bool IsBodyHtml { get; set; }
        public bool EnableSsl { get; set; }
        public string Message { get; set; }
    }
}