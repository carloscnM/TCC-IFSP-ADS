using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Login
{

    public class RegistrarViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório"), StringLength(100, MinimumLength = 5, ErrorMessage = "Nome deve ter no mínimo 5 e no máximo 100 letras"), Display(Name = "Nome Completo")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "E-mail é obrigatório!"), EmailAddress, Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha é Obrigatório"), DataType(DataType.Password)]
        public string Senha { get; set; }
        [Compare("Senha", ErrorMessage = "As senhas não são iguais"), Display(Name = "Confirme sua Senha"), DataType(DataType.Password)]
        public string ConfirmeSenha { get; set; }
    }
}
