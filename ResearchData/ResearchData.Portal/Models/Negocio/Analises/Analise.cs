using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.Models.Projetos.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.Negocio.Analises
{
    [DataContract(), Table("ANALISE")]
    public class Analise
    {
        [Key, Column("ana_id"), DataMember()]
        public int Id { get; set; }


        [Column("ana_nome"), DataMember()]
        public string Nome { get; set; }


        [Column("ana_descricao"), DataMember()]
        public string Descricao { get; set; }


        [Column("ana_datainicio"), DataMember()]
        public DateTime DateInicio { get; set; }


        [Column("ana_datafim"), DataMember()]
        public DateTime? DataFim { get; set; }


        [Column("ana_ativa"), DataMember()]
        public bool Ativa { get; set; }


        [Column("PROJETO_pro_id"), DataMember()]
        public int ProjetoId { get; set; }
        public virtual Projeto Projeto { get; set; }


        public virtual List<ColaboradorAnalise> ColaboradorAnalise { get; set; }
    }
}
