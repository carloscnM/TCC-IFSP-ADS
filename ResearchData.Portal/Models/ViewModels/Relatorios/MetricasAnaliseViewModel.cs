using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Relatorios
{
    public class MetricasAnaliseViewModel
    {
        public double TotalMedicoes { get; set; }
        public double MedicoesCaptadas { get; set; }

        public double MediaMedicoesPorSujeito { get; set; }
        public double MediaMedicoesPorExperimento { get; set; }

    }
}
