using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchData.Portal.Models.ViewModels.Admin;

namespace ResearchData.Portal.Data.Repositorio.Admin
{
    public interface IAdminRepositorio
    {
        bool EhUsuarioCadastrado(string email);
        AtribuirDireitoViewModel BuscarDadosUsuario(string email, IList<RolesUsarioViewModel> listaPermissoes);
    }
}
