using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Projetos
{
    public class DetalhesProjetoViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public DateTime DtInicio { get; set; }

        public DateTime? DtFim { get; set; }

        public int QuatidadeAnalise { get; set; }
    }
}
