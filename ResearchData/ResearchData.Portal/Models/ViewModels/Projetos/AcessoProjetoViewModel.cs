using ResearchData.Portal.Models.Negocio.Analises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Projetos
{
    public class AcessoProjetoViewModel : BaseTituloDescricaoViewModel
    {
        public string Id { get; set; }
        public string DataCriacao { get; set; }
        public DateTime? DataFinal { get; set; }

        public IList<Analise> ListaAnalise { get; set; }

        public bool AnalisesAberta { get; set; }
    }
}
