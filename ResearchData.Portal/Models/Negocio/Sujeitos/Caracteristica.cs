using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.Negocio.Sujeitos
{
    [DataContract(), Table("CARACTERISTICA")]
    public class Caracteristica
    {
        

        [Key, Column("car_id"), DataMember()]
        public int Id { get; set; }


        [Column("car_descricao"), DataMember()]
        public string Descricao { get; set; }

        [Column("car_comun"), DataMember()]
        public bool Comun { get; set; }

        [Column("car_tipo"), DataMember()]
        public virtual TipoDoDado TipoDoDado { get; set; }

    }


    public  enum TipoDoDado
    {
        tint,
        tfloat,
        tstring,
        tdata,
        tbool
    }
}
