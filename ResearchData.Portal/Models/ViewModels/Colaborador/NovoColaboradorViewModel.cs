using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Colaborador
{
    public class NovoColaboradorViewModel
    {
        public int IdAnalise { get; set; }
        public int IdProjeto { get; set; }
        public string Email { get; set; }

        public int Acesso { get; set; }
    }
}
