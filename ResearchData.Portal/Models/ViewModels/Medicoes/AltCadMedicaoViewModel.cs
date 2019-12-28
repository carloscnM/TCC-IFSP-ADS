using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Medicoes
{
    public class AltCadMedicaoViewModel
    {
        public int IdSujeito { get; set; }

        public string DescricaoSujeito { get; set; }

        public int IdAnalise { get; set; }

        public int IdProjeto { get; set; }

        public IList<MedicoesViewModel> ListaMedicoes { get; set; }
    }
}
