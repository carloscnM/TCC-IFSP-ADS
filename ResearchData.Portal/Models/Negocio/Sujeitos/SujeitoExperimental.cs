using ResearchData.Portal.Models.Negocio.Analises;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.Negocio.Sujeitos
{
    [DataContract(), Table("SUJEITOEXPERIMENTAL")]
    public class SujeitoExperimental
    {
        [Key, Column("sujexp_id"), DataMember()]
        public int Id { get; set; }

        [Column("sujexp_descricao"), DataMember()]
        public string Descricacao { get; set; }

        [Column("sujexp_idexterno")]
        public int? Externo { get; set; }


        [Column("GRUPO_gru_id"), DataMember()]
        public int? GrupoId { get; set; }
        public virtual Grupo Grupo { get; set; }

       

        [Column("ANALISE_ana_id"), DataMember()]
        public int AnaliseId { get; set; }
        public Analise Analise { get; set; }

        

        public virtual List<Medicao> Medicao { get; set; }
    }
}
