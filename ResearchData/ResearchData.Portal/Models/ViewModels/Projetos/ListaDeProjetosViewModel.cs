using ResearchData.Portal.Models.Projetos.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Projetos
{
    public class ListaDeProjetosViewModel
    {
        public IEnumerable<Projeto> ProjetosProprios { get; set; }
        public IEnumerable<Projeto> ProjetosCompartilhados { get; set; }
    }
}
