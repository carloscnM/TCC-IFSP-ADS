using ResearchData.Portal.Models.Projetos.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.Negocio.Sujeitos
{
    [DataContract(), Table("GRUPO")]
    public class Grupo
    {
        [Key, Column("gru_id"), DataMember()]
        public int Id { get; set; }

        [Column("gru_nome"), DataMember()]
        public string Nome { get; set; }

        [Column("gru_descricao"), DataMember()]
        public string Descricao { get; set; }

        [Column("gru_datainclusao")]
        public DateTime DataInclusao { get; set; }

        [Column("gru_analiseorigem")]
        public int AnaliseOrigem { get; set; }

        [Column("gru_ativo")]
        public bool Ativo { get; set; }

        [Column("PROJETO_pro_id")]
        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; }
    }
}
