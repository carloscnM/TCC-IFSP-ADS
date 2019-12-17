using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório"), Display(Name = "E-mail"), EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha é obrigatório"), DataType(DataType.Password), Display(Name = "Senha")]
        public string Senha { get; set; }

        public bool LembreMe { get; set; }
    }
}
