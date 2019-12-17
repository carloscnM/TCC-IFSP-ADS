using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Projetos
{
    public class DadosSimplesProjetoViewModel
    {
        public int IdProjeto { get; set; }
        public string TituloProjeto { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
