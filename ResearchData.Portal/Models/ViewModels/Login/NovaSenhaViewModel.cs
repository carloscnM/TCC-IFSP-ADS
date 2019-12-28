using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Login
{
    public class NovaSenhaViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string UsuarioId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Token { get; set; }

        [Required(ErrorMessage = "Senha Obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NovaSenha { get; set; }
        [Compare("NovaSenha", ErrorMessage = "Senhas estão diferente"),Display(Name = "Confirme sua senha"),DataType(DataType.Password)]
        public string ConfirmarNovaSenha { get; set; }

    }
}
