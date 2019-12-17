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
    [DataContract(), Table("COMENTARIO")]
    public class Comentario
    {
        [Key, ColumnAttribute("com_id"), DataMember()]
        public int Id { get; set; }


        [Column("com_comentario"), DataMember()]
        public string TextoComentario { get; set; }


        [Column("com_datainclusao"), DataMember()]
        public DateTime DataInclusao { get; set; }


        [Column("ANALISE_ana_id"), DataMember()]
        public int AnaliseId { get; set; }
        public virtual Analise Analise { get; set; }



        [Column("USUARIO_usu_id"), DataMember()]
        public string UsuarioAplicacaoId { get; set; }
        public virtual UsuarioAplicacao UsuarioAplicacao { get; set; }
    }
}
