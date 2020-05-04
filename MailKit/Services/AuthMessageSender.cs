using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MailKit.Services
{
    public class AuthMessageSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public AuthMessageSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message, IFormFile attachment)
            => await ExecuteAsync(email, subject, message, attachment);

        private async Task ExecuteAsync(string email, string subject, string message, IFormFile attachment)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UserNameEmail, _emailSettings.UserNamePassword);
                    smtp.EnableSsl = _emailSettings.EnableSsl;
                    await smtp.SendMailAsync(CreateMailMessage(email, subject, message,attachment));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MailMessage CreateMailMessage(string email, string subject, string message, IFormFile attachment)
        {
            string toEmail = string.IsNullOrEmpty(email) ? _emailSettings.ToEmail : email;
            var mail = new MailMessage()
            {
                From = new MailAddress(_emailSettings.UserNameEmail, _emailSettings.DisplayName),
                Subject = subject,
                Body = message,
                IsBodyHtml = _emailSettings.IsBodyHtml,
                Priority = _emailSettings.Priority,
            };
            mail.To.Add(new MailAddress(toEmail));
            if (!string.IsNullOrEmpty(_emailSettings.CcEmail))
                mail.CC.Add(new MailAddress(_emailSettings.CcEmail));
            if (attachment != null)
                mail.Attachments.Add(new Attachment(attachment.OpenReadStream(), attachment.FileName, attachment.ContentType));
            return mail;
        }
    }
}
