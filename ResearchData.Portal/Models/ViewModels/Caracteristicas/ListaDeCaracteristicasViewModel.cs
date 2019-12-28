using ResearchData.Portal.Models.Negocio.Sujeitos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Caracteristicas
{
    public class ListaDeCaracteristicasViewModel
    {
        public int CaracteristicaId { get; set; }
        public string Descricao { get; set; }
        public TipoDoDado Tipo { get; set; }

        public bool IsChecked { get; set; }
    }
}
