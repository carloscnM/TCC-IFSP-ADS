using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels
{
    public class BaseTituloDescricaoViewModel
    {
        [Required, MaxLength(250), MinLength(5)]
        public string Titulo { get; set; }
        [Required, MaxLength(250), MinLength(5)]
        public string Descricacao { get; set; }
    }
}
