using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.DadosDeCadastro
{
    public class DadosDeCadastroViewModel
    {
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Data de Cadastro")]
        public DateTime DataDeCadastro { get; set; }
        [Display(Name = "Último Acesso")]
        public DateTime DataUltimoAcesso { get; set; }
    }
}

