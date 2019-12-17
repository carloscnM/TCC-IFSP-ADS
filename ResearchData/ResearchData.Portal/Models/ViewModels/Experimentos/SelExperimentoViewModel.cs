using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Experimentos
{
    public class SelExperimentoViewModel
    {
        public string TituloAnalise { get; set; }
        public int IdAnalise { get; set; }
        public int IdProjeto { get; set; }
        public int IdExperimento { get; set; }
        public IList<ExpSimplesViewModel> ListaExp { get; set; }
    }
}
