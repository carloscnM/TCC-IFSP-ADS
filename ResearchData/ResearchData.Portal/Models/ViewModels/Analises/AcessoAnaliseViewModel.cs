using ResearchData.Portal.Models.ViewModels.Projetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Analises
{
    public class AcessoAnaliseViewModel : BaseTituloDescricaoViewModel
    {
        public int IdAnalise { get; set; }
        public int projetoId { get; set; }
        public DateTime Criacao { get; set; }
        public DateTime? Encerramento { get; set; }

        public string TituloProjeto { get; set; }

        public bool TemSujeito { get; set; }

        public int Acesso { get; set; }

        public ResponsavelDoProjetoViewModel Responsavel { get; set; }
    }
}
