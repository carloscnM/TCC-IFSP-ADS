using ResearchData.Portal.Models.ViewModels.Sujeitos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Medicoes
{
    public class AltCadMedicaoExpViewModel
    {
        public int IdAnalise { get; set; }
        public string TituloAnalise { get; internal set; }

        public int IdProjeto { get; set; }

        public int IdExperimento { get; set; }
        public string NomeExp { get; set; }

        public IList<ListaSujeitoSimplesViewModel> SujeitosNoExperimento { get; set; }
        public IList<ListaSujeitoSimplesViewModel> SujeitosDisponiveis { get; set; }
        
    }
}
