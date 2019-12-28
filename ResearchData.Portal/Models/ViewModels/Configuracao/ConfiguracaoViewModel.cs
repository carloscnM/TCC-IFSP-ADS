using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Configuracao
{
    public class ConfiguracaoViewModel
    {
        [Required, MaxLength(10)]
        public string Codigo { get; set; }
        [Required, MaxLength(250)]
        public string Nome { get; set; }
        [Required, MaxLength(8)]
        public string Sigla { get; set; }
        [Required, MaxLength(61)]
        public string Campus { get; set; }
    }
}
