using ResearchData.Portal.Models.ViewModels.Analises;
using ResearchData.Portal.Models.ViewModels.Projetos;
using ResearchData.Portal.Models.ViewModels.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Relatorio
{
    public class RelatorioDeAnaliseViewModel
    {
        public DetalhesProjetoViewModel DetalheProjeto { get; set; }

        public DetalheAnaliseViewModel DetalheAnalise { get; set; }

        public MetricasAnaliseViewModel Metricas { get; set; }

        public IList<SujeitosPorExperimentoViewModel> SujeitosPorExperimento { get; set; }

        public IList<SujeitosPorGrupoViewModel> SujeitosPorgrupo { get; set; }
    }
}
