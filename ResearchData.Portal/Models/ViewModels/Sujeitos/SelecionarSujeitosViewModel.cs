using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Caracteristicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Sujeitos
{
    public class SelecionarSujeitosViewModel
    {
        public int IdAnalise { get; set; }

        public string TituloAnalise { get; set; }

        public int IdProjeto { get; set; }


        public IList<ListaDeCaracteristicasViewModel> ListaCaracteristicaNSelecionadas { get; set; }

        public IList<ListaDeCaracteristicasViewModel> ListaCaracteristicaSelecionadas { get; set; }
    }
}
