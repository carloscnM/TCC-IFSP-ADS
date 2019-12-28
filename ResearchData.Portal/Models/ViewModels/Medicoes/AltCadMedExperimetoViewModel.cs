using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Medicoes
{
    public class AltCadMedExperimetoViewModel
    {
        public int IdSujeito { get; set; }
        public string DescSujeito { get; set; }

        public int IdAnalise { get; set; }
        public int IdProjeto { get; set; }

        public int IdExperimento { get; set; }

        public string NomeExperimento { get; set; }
        public IList<MedicoesViewModel> ListaMedicoes { get; set; }
    }
}
