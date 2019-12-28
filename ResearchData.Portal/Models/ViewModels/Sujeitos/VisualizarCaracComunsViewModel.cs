using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Sujeitos
{
    public class VisualizarCaracComunsViewModel
    {
        public int IdSujeito { get; set; }
        public string DescricaoSujeito { get; set; }

        public IEnumerable<MedicaoComunsViewModel> MedicaoVM { get; set; }
    }
}
