using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.Negocio.Experimentos
{
    [DataContract(), Table("EXPERIMENTO")]
    public class Experimento
    {
        [Key, Column("exp_id"), DataMember()]
        public int Id { get; set; }

        [Column("exp_nome"), DataMember()]
        public string Nome { get; set; }


        [Column("exp_descricao"), DataMember()]
        public string Descricao { get; set; }


        public List<TemplateExperimento> TempleteExperimento { get; set; }
    }
}
