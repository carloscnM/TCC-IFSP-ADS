using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.Data.Repositorio.Admin;
using ResearchData.Portal.GerenciamentoUsuario.PerfisDeUsuario;
using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.Models.ViewModels.Admin;
using Microsoft.Extensions.Localization;

namespace ResearchData.Portal.Controllers.Admin
{
    [Authorize(Roles = PerfisPadroes.ADMINISTRADOR)]
    public class AdminController : Controller
    {
        private IStringLocalizer<AdminController> _localizador;
        private IAdminRepositorio _repoAdmin;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<UsuarioAplicacao> _userManager;

        public AdminController(IAdminRepositorio adminRepositorio,
                                UserManager<UsuarioAplicacao> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                        IStringLocalizer<AdminController> localizador)
        {
            this._repoAdmin = adminRepositorio;
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._localizador = localizador;
        }
        public IActionResult Direitos()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Direitos(UsuarioEmailViewModel modelo)
        {
            if (ModelState.IsValid & modelo != null)
            {
                bool existe = _repoAdmin.EhUsuarioCadastrado(modelo.Email);
                if (existe)
                {
                    return RedirectToAction(nameof(AtribuirDireito), modelo);
                }

                ModelState.AddModelError("", _localizador["Usuario não Cadastrado"]);
                return View();
            }
            
            ModelState.AddModelError("", _localizador["É necessário informar um e-mail válido"]);
            return View();
        }


        public IActionResult AtribuirDireito(UsuarioEmailViewModel modelo)
        {
            if (ModelState.IsValid & modelo != null)
            {
                IList<RolesUsarioViewModel> listaPermissoes = new List<RolesUsarioViewModel>
                {
                    new RolesUsarioViewModel() { NomeRoles = PerfisPadroes.ADMINISTRADOR, Ativa = false, Descricao = _localizador["Acesso Administrativo"].ToString() },
                    new RolesUsarioViewModel() { NomeRoles = PerfisPadroes.CRIARCATEGORIA, Ativa = false, Descricao = _localizador["Manutenção de Caracteríticas"].ToString() },
                    new RolesUsarioViewModel() { NomeRoles = PerfisPadroes.CRIARTEMPLETE, Ativa = false, Descricao = _localizador["Manutençao de modelo de Experimento"].ToString() }
                };
                AtribuirDireitoViewModel modeloRetorno = _repoAdmin.BuscarDadosUsuario(modelo.Email, listaPermissoes);
                return View(modeloRetorno);
            }
            return RedirectToAction(nameof(Direitos));
        }

        [HttpPost]
        public async Task<IActionResult> AtribuirDireito(AtribuirDireitoViewModel modelo)
        {
            if (ModelState.IsValid & modelo != null)
            {
                var usuario = await _userManager.FindByEmailAsync(modelo.Email).ConfigureAwait(false);

                var remover = modelo.RolesDeAcesso.Where(ro => ro.Ativa == false).Select(ro => ro.NomeRoles);
                var add = modelo.RolesDeAcesso.Where(ro => ro.Ativa == true).Select(ro => ro.NomeRoles);

                if (remover.Count() > 0)
                {
                    foreach (var item in remover)
                    {
                        await _userManager.RemoveFromRoleAsync(usuario, item).ConfigureAwait(false);
                    }
                }

                if (add.Count() > 0)
                {
                    foreach (var item in add)
                    {
                        await _userManager.AddToRoleAsync(usuario, item).ConfigureAwait(false);
                    }
                }


                ViewData["MsgReposta"] = 1;
                return View();

            }
            ModelState.AddModelError("", _localizador["Ocorreu um erro, tente novamente ou acesso mais tarde"]);
            return View(modelo);
        }

    }
}