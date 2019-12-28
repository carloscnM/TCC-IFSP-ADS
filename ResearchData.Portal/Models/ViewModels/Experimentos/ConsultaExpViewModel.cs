using ResearchData.Portal.Models.ViewModels.Caracteristicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Experimentos
{
    public class ConsultaExpViewModel
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public IList<ListaDeCaracteristicasViewModel> Lista { get; set; }
    }
}
