using ResearchData.Portal.Models.ViewModels.Sujeitos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Medicoes
{
    public class MedicaoExpViewModel
    {
        public int IdSujeito { get; set; }

        public string DescricaoSujeito { get; set; }

        public int IdExperimento { get; set; }

        public string NomeExperimento { get; set; }

        public IEnumerable<MedicaoComunsViewModel> MedicaoVM { get; set; }
    }
}
