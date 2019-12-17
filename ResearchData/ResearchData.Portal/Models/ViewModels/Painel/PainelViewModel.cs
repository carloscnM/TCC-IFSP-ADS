using ResearchData.Portal.Models.ViewModels.Projetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Painel
{
    public class PainelViewModel
    {
        public IList<DadosSimplesProjetoViewModel> ProjetosRecente { get; set; }
        public int QuantidadeProjetoAnd { get; set; }
        public int QuantidadeProjetoEnc { get; set; }
        public int QuantidadeColaboracoes { get; set; }
    }
}
