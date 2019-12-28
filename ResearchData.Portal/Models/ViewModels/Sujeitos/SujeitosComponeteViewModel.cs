using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Sujeitos
{
    public class SujeitosComponeteViewModel
    {
        public int IdAnalise { get; set; }
        public int IdProjeto { get; set; }
        public bool AnaliseAtiva { get; set; }
        public IEnumerable<ListaSujeitosViewModel> Sujeitos { get; set; }
    }
}
