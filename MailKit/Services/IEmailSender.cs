using System.Collections;
using System.Collections.Generic;
using MailKit.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MailKit.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, IFormFile attachment);
        Task SendEmailApiAsync(Email email);
        Task SendEmailApiAsync(IEnumerable<Email> emails);
    }
}