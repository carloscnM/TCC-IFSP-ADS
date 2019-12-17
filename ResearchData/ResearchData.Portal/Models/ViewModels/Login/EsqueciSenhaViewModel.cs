using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Login
{
    public class EsqueciSenhaViewModel
    {
        [Required(ErrorMessage = "E-mail Obrigatório"), EmailAddress(ErrorMessage = "E-mail Inválido")]
        public string Email { get; set; }
    }
}
