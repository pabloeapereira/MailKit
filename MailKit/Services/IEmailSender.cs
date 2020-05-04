﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MailKit.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, IFormFile attachment);
    }
}