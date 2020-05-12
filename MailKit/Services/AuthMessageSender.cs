using MailKit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public async Task SendEmailApiAsync(Email email)
            => await ExecuteApiAsync(email);
        public async Task SendEmailApiAsync(IEnumerable<Email> emails)
            => await ExecuteApiAsync(emails);


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

        private async Task ExecuteApiAsync(Email email)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient(email.PrimaryDomain, email.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(email.UserNameEmail, email.UserNamePassword);
                    smtp.EnableSsl = email.EnableSsl;
                    await smtp.SendMailAsync(CreateApiMailMessage(email));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task ExecuteApiAsync(IEnumerable<Email> emails)
        {
            try
            {
                using var smtp = new SmtpClient(emails.FirstOrDefault().PrimaryDomain,
                    emails.FirstOrDefault().PrimaryPort)
                {
                    Credentials = new NetworkCredential(emails.FirstOrDefault().UserNameEmail,
                        emails.FirstOrDefault().UserNamePassword),
                    EnableSsl = emails.FirstOrDefault().EnableSsl
                };
                foreach (var email in emails)
                {
                    await smtp.SendMailAsync(CreateApiMailMessage(email));
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

        private MailMessage CreateApiMailMessage(Email email)
        {
            var mail = new MailMessage()
            {
                From = new MailAddress(email.UserNameEmail, email.DisplayName),
                Subject = email.Subject,
                Body = email.Message,
                IsBodyHtml = email.IsBodyHtml,
                Priority = email.Priority,
            };
            mail.To.Add(new MailAddress(email.ToEmail));
            if (!string.IsNullOrEmpty(email.CcEmail))
                mail.CC.Add(new MailAddress(email.CcEmail));
            if(!string.IsNullOrEmpty(email.BccEmail))
                mail.Bcc.Add(new MailAddress(email.CcEmail));
            return mail;
        }
    }
}
