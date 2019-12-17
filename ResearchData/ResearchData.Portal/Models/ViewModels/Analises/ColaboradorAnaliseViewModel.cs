using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Analises
{
    public class ColaboradorAnaliseViewModel
    {
        public int IdAnalise { get; set; }

        public IEnumerable<ListaColaboradorViewModel> Lista { get; set; }
        public int IdProjeto { get; internal set; }
    }
}
