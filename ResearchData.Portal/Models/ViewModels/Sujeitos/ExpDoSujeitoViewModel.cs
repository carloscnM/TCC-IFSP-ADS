using ResearchData.Portal.Models.ViewModels.Experimentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Sujeitos
{
    public class ExpDoSujeitoViewModel
    {
        public int IdSujeito { get; set; }
        public string NomeSujeito { get; set; }
        public int IdAnalise { get; set; }
        public int IdProjeto { get; set; }

        public bool AnaliseAtiva { get; set; }

        public int ExperimentoId { get; set; }
        public IList<ExpSimplesViewModel> ListaExp { get; set; }
    }
}
