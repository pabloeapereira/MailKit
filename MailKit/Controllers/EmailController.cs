using MailKit.Models;
using MailKit.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MailKit.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EmailViewModel email)
        {
            try
            {
                await SendEmailMessageAsync(email.Destino, email.Assunto, email.Mensagem,email.File);
                return RedirectToAction(nameof(EmailSended));
            }
            catch(Exception)
            {
                return RedirectToAction(nameof(EmailFail));
            }
        }

        private async Task SendEmailMessageAsync(string email, string subject, string message, IFormFile attachment)
        {

            await _emailSender.SendEmailAsync(email, subject, message, attachment);
        }

        public IActionResult EmailSended()
        {
            return View();
        }
        public IActionResult EmailFail()
        {
            return View();
        }
    }
}
