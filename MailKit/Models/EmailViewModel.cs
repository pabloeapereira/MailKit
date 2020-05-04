using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MailKit.Models
{
    public class EmailViewModel
    {
        [Required,DisplayName("E-mail de destino"), EmailAddress]
        public string Destino { get; set; }

        [Required, DisplayName("Assunto")]
        public string Assunto { get; set; }

        [Required, DisplayName("Mensagem")]
        public string Mensagem { get; set; }
        [DisplayName("Anexo")]
        public IFormFile File { get; set; }
    }
}
