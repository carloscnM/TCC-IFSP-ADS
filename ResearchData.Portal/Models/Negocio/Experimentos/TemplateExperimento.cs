using ResearchData.Portal.Models.Negocio.Sujeitos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.Negocio.Experimentos
{
    [DataContract(), Table("GRUPODEDADOS")]
    public class TemplateExperimento
    {
        [Key, Column("grudad_id"), DataMember()]
        public int Id { get; set; }


        [Column("EXPERIMENTO_exp_id"), DataMember()]
        public int ExperimentoId { get; set; }
        public virtual Experimento Experimento { get; set; }


        [Column("CARACTERISTICA_car_id"), DataMember()]
        public int CaracteristicaId { get; set; }
        public Caracteristica Caracteristica { get; set; }
    }
}
