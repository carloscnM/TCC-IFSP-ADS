using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.DadosDeCadastro
{
    public class AlterarSenhaViewModel
    {
        [DataType(DataType.Password), Required(ErrorMessage = "Senha atual obrigatória"), Display(Name = "Senha Atual")]
        public string SenhaAtual { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Nova senha obrigatória"), Display(Name = "Nova Senha")]
        public string SenhaNova { get; set; }
        [DataType(DataType.Password), Compare("SenhaNova", ErrorMessage = "Senha diferente"), Display(Name = "Confirme a nova senha")]
        public string ConfirmarSenhaNova { get; set; }
    }
}
