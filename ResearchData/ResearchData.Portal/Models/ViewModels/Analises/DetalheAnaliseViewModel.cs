using ResearchData.Portal.Models.ViewModels.Colaborador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Analises
{
    public class DetalheAnaliseViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DtInicio { get; set; }
        public DateTime? DtFim { get; set; }
        public IList<DetalheColaboradorViewModel> Colaboradores { get; set; }

        public int QuatidadeDias { get; set; }
        public int QuatidadeSujeito { get; set; }
        public int QuantidadeGrupo { get; set; }
        public int QuatidadeExp { get; set; }
    }
}
