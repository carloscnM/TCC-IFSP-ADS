using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Analises
{
    public class NovaAnaliseViewModel : BaseTituloDescricaoViewModel
    {
        public int IdAnalise { get; set; }
        [Required]
        public int ProjetoId { get; set; }
    }
}
