using ResearchData.Portal.Models.Negocio.Analises;
using ResearchData.Portal.Models.Negocio.Experimentos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.Negocio.Sujeitos
{
    [DataContract(), Table("MEDICAO")]
    public class Medicao
    {
        [Key, ColumnAttribute("med_id"), DataMember()]
        public int Id { get; set; }

        [Column("med_datacaptacao"), DataMember()]
        public DateTime? DataCaptacao { get; set; }


        [Column("med_datamodificacao"), DataMember()]
        public DateTime? DataModificacao { get; set; }


        [Column("med_caracteristicadouble"), DataMember()]
        public double? MedDouble { get; set; }


        [Column("med_caracteristicadata"), DataMember()]
        public DateTime? MedData { get; set; }


        [Column("med_caracteristicaint"), DataMember()]
        public int? MedInt { get; set; }


        [Column("med_caracteristicastring"), DataMember()]
        public string MedString { get; set; }


        [Column("med_caracteristicabool"), DataMember()]
        public bool? MedBool { get; set; }


        [Column("SUJEITO_sujexp_id"), DataMember()]
        public int SujeitoExperimentalId { get; set; }
        public virtual SujeitoExperimental SujeitoExperimental { get; set; }


        [Column("CARACTERISTICA_car_id"), DataMember()]
        public int CaracteristicaId { get; set; }
        public virtual Caracteristica Caracteristica { get; set; }


        [Column("EXPERIMENTO_exp_id"), DataMember()]
        public int? ExperimentoId { get; set; }
        public Experimento Experimento { get; set; }

        [Column("ANALISE_ana_id"), DataMember()]
        public int? AnaliseId { get; set; }
        public Analise Analise { get; set; }
    }
}
