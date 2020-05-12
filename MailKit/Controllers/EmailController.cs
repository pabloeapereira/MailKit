using MailKit.Models;
using MailKit.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailKit.Controllers
{
    [Route("email")]
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
                await SendEmailMessageAsync(email.Destino, email.Assunto, email.Mensagem, email.File);
                return RedirectToAction(nameof(EmailSended));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(EmailFail));
            }
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody]Email email)
        {
            try
            {
                await _emailSender.SendEmailApiAsync(email);
                return CustomResponse(email);
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                errors.Add(ex.Message);
                return CustomResponse(null, "Error", errors);
            }
        }

        [HttpPost("send-many")]
        public async Task<IActionResult> SendEmails([FromBody] IEnumerable<Email> emails)
        {
            try
            {
                await _emailSender.SendEmailApiAsync(emails);
                return CustomResponse();
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                errors.Add(ex.Message);
                return CustomResponse(null, "Error", errors);
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

        protected IActionResult CustomResponse(object result = null, string situacao = "OK", List<string> errors = null)
        {

            if (situacao == "OK")
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    errors = errors
                });

            }

        }
    }
}
