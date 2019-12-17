using ResearchData.Portal.Models.Negocio.Sujeitos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Sujeitos
{
    public class NovoSujeitoViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(61, MinimumLength = 1), Display(Name = "Nome ou Descricão")]
        public string Descricao { get; set; }

        public int AnaliseId { get; set; }

        public int ProjetoId { get; set; }

        public string GrupoNome { get; set; }

        public int? GrupoId { get; set; }

        public virtual IEnumerable<Grupo> Grupos { get; set; }
    }
}
