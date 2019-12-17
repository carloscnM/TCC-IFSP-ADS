using ResearchData.Portal.Models.Negocio.Sujeitos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Caracteristicas
{
    public class CriarCaracteristicaViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public virtual TipoDoDado TipoDoDado { get; set; }
        
        [Required]
        public bool Comun { get; set; }
    }
}
