using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Colaborador
{
    public class DetalheColaboradorViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DtInclusao { get; set; }
        public int IdAnalise { get; set; }
        public int IdProjeto { get; set; }
    }
}
