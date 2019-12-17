using ResearchData.Portal.GerenciamentoUsuario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.Projetos.Negocio
{
    [Table("PROJETO"), DataContract()]
    public class Projeto
    {
        [Key, ColumnAttribute("pro_id"), DataMember()]
        public int Id { get; set; }


        [ColumnAttribute("pro_titulo"), Required, DataMember()]
        public string Titulo { get; set; }


        [ColumnAttribute("pro_descricao"), Required, DataMember()]
        public string Descricao { get; set; }


        [Column("pro_datainicio"), DataMember()]
        public DateTime DataInicial { get; set; }


        [ColumnAttribute("pro_datafim"), DataMember()]
        public DateTime? DataFinal { get; set; }


        [ColumnAttribute("pro_ativo"), DataMember()]
        public bool Ativo { get; set; }


        [ColumnAttribute("USUARIO_usu_id"), DataMember(), Required]
        public string UsuarioAplicacaoId { get; set; }
        public virtual UsuarioAplicacao UsuarioAplicacao { get; set; }
    }
}
