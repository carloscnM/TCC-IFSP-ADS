using ResearchData.Portal.Models.Negocio.Sujeitos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Sujeitos
{
    public class SujeitoExistenteViewModel
    {
        public int IdAnalise { get; set; }

        public int IdProjeto { get; set; }

        public int? GrupoId { get; set; }

        public virtual IEnumerable<Grupo> Grupos { get; set; }

        public IList<ListaSujeitoSimplesViewModel> ListaSujeito { get; set; }
    }
}
