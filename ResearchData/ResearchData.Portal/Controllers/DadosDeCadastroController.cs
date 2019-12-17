using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.Models.ViewModels.DadosDeCadastro;
using Microsoft.Extensions.Localization;

namespace ResearchData.Portal.Controllers
{
    [Authorize]
    public class DadosDeCadastroController : Controller
    {
        private readonly UserManager<UsuarioAplicacao> _userManager;
        private readonly IStringLocalizer<DadosDeCadastroController> _localizador;

        public DadosDeCadastroController(UserManager<UsuarioAplicacao> userManager, IStringLocalizer<DadosDeCadastroController> localizador)
        {
            _userManager = userManager;
            _localizador = localizador;
        }


        public async Task<IActionResult> DadosUsuario()
        {
            var user = await _userManager.GetUserAsync(User);
            var modelo = new DadosDeCadastroViewModel()
            {
                Nome = user.Nome,
                Email = user.Email,
                DataDeCadastro = user.DataCadastro,
                DataUltimoAcesso = user.DataUltimoAcesso
            };
            return View(modelo);
        }

        public IActionResult AlteraSenha ()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AlteraSenha(AlterarSenhaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.GetUserAsync(User);
                var resultadoTrocaSenha = await _userManager.ChangePasswordAsync(usuario, modelo.SenhaAtual, modelo.SenhaNova);
                if (resultadoTrocaSenha.Succeeded)
                {
                    return RedirectToAction("DadosUsuario", "DadosDeCadastro");
                }
                else
                {
                    ModelState.AddModelError("", _localizador["Senha atual incorreta ou algo deu errado"]);
                }
                
            }
            return View(modelo);
        }


    }
}