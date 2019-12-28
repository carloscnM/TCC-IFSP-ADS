using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.GerenciamentoUsuario.PerfisDeUsuario;
using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio.Admin
{
    public class AdminRepositorio : BaseRepositorio<UsuarioAplicacao>, IAdminRepositorio
    {
        public AdminRepositorio(RDContextoDaAplicacao contexto) : base(contexto)
        {

        }

        public AtribuirDireitoViewModel BuscarDadosUsuario(string email, IList<RolesUsarioViewModel> listaPermissoes)
        {
            AtribuirDireitoViewModel modelo = new AtribuirDireitoViewModel();

            var userSimples = contexto.Users.Where(usu => usu.Email == email).FirstOrDefault();

            modelo.UserId = userSimples.Id;
            modelo.Nome = userSimples.Nome;
            modelo.Email = userSimples.Email;
            modelo.DataDeCadastro = userSimples.DataCadastro;

            var rolesUsuario = from role in contexto.Roles
                               join roleUse in contexto.UserRoles on role.Id equals roleUse.RoleId
                               where roleUse.UserId == userSimples.Id & role.NormalizedName != PerfisPadroes.USUARIO
                               select role.NormalizedName;

            
            modelo.RolesDeAcesso = listaPermissoes;

           

            foreach (var item in modelo.RolesDeAcesso)
            {
                if (rolesUsuario.Contains(item.NomeRoles))
                    item.Ativa = true;
            }



            return modelo;
        }

        public bool EhUsuarioCadastrado(string email)
        {
            var existe = contexto.Users.Where(usu => usu.Email == email).Count();
            if (existe > 0)
                return true;
            return false;
        }
    }
}
