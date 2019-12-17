using ResearchData.Portal.GerenciamentoUsuario.PerfisDeUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Admin
{
    public class AtribuirDireitoViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public IList<RolesUsarioViewModel> RolesDeAcesso { get; set; }
    }
}
