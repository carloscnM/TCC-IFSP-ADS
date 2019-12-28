using ResearchData.Portal.Models.Negocio.Sujeitos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Medicoes
{
    public class MedicoesViewModel
    {
        public int IdMedicao { get; set; }
        public string DescricaoMedicao { get; set; }
        public string ResultadoMedicao { get; set; }
        public DateTime? DataCaptacao { get; set; }
        public TipoDoDado TipoDoDado { get; set; }
        public string TipoHtml { get; set; }
        public int? IdExperimento { get; set; }
    }
}
