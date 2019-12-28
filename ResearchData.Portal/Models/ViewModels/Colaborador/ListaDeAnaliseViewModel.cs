using ResearchData.Portal.Models.Negocio.Analises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Colaborador
{
    public class ListaDeAnaliseViewModel
    {
        public int IdProjeto { get; set; }
        public string TituloProjeto { get; set; }
        public IEnumerable<Analise> Analises { get; set; }
    }
}
