using System.Net.Mail;

namespace MailKit.Services
{
    public class EmailSettings
    {
        public string PrimaryDomain { get; set; }
        public int PrimaryPort { get; set; }
        public string UserNameEmail { get; set; }
        public string UserNamePassword { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string DisplayName { get; set; }
        public MailPriority Priority { get; set; }
        public bool IsBodyHtml { get; set; }
        public bool EnableSsl { get; set; }
    }
}