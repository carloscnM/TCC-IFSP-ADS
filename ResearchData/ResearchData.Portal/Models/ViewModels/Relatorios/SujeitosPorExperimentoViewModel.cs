using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Relatorios
{
    public class SujeitosPorExperimentoViewModel
    {
        public string NomeExperimento { get; set; }

        public IList<string> Sujeitos { get; set; }
    }
}
