using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Grupos
{
    public class GrupoViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 1), Display(Name = "Nome do Grupo")]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public int ProjetoId { get; set; }

        public int AnaliseId { get; set; }

        public static bool Resposavel { get; set; }
    }
}
