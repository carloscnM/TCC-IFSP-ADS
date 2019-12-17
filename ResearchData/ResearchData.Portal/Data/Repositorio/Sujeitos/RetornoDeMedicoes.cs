using System;
using ResearchData.Portal.Models.Negocio.Sujeitos;

namespace ResearchData.Portal.Data.Repositorio.Sujeitos
{
    public class RetornoDeMedicoes
    {
        public int IdMedicao { get; set; }
        public string NomeCaract { get; set; }
        public DateTime? DataCaptacao { get; set; }
        public int? Resultadoint { get; set; }
        public bool? ResultadoBool { get; set; }
        public string ResultadoData { get; set; }
        public string ResultadoDouble { get; set; }
        public string ResultadoString { get; set; }
        public int? IdExperimento { get; set; }
        public TipoDoDado Tipo { get; set; }
    }
}