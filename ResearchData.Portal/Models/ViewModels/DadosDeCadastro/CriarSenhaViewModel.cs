using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.DadosDeCadastro
{
    public class CriarSenhaViewModel
    {
        [Required, DataType(DataType.Password)]
        public string Senha { get; set; }
        [Compare("Senha"), Display(Name = "Confirme sua senha"), DataType(DataType.Password)]
        public string CompararSenha { get; set; }
    }
}
