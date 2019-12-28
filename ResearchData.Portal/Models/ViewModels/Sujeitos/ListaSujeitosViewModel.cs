using ResearchData.Portal.Models.Negocio.Sujeitos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Sujeitos
{
    public class ListaSujeitosViewModel
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public int IdProjeto { get; set; }

        public int IdAnalise { get; set; }

        public int GrupoId { get; set; }

        public string GrupoNome { get; set; }
    }
}
