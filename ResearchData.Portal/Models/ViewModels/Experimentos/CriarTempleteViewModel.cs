using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Caracteristicas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Experimentos
{
    public class CriarTempleteViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public IList<ListaDeCaracteristicasViewModel> Caracteristicas { get; set; }
    }
}
