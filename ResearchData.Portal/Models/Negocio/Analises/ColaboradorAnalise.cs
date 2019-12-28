using ResearchData.Portal.GerenciamentoUsuario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.Negocio.Analises
{
    [Table("COLABORADOR"), DataContract()]
    public class ColaboradorAnalise
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("col_datainclusao")]
        public DateTime DataInclusao { get; set; }

        [Column("col_acesso")]
        public int Acesso { get; set; }


        [Column("USUARIO_usu_id"), DataMember()]
        public string UsuarioAplicacaoId { get; set; }
        public virtual UsuarioAplicacao UsuarioAplicacao { get; set; }


        [Column("ANALISE_ana_id"), DataMember()]
        public int AnaliseId { get; set; }
        public virtual Analise Analise { get; set; }

    }
}
