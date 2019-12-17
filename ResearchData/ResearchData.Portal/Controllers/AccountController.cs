using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.GerenciamentoUsuario.PerfisDeUsuario;
using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.Models.ViewModels.DadosDeCadastro;
using ResearchData.Portal.Models.ViewModels.Login;

namespace ResearchData.Portal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IStringLocalizer<AccountController> _localizador;
        private readonly UserManager<UsuarioAplicacao> _userManager;
        private readonly SignInManager<UsuarioAplicacao> _signInManager;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<UsuarioAplicacao> userManager,
                               SignInManager<UsuarioAplicacao> signInManager,
                               ILogger<AccountController> logger,
                               IStringLocalizer<AccountController> localizador,
                               IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _localizador = localizador;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (ModelState.IsValid & modelo != null)
            {
                var resultado = await _signInManager.PasswordSignInAsync(modelo.Email, modelo.Senha, modelo.LembreMe, lockoutOnFailure: false).ConfigureAwait(false);
                if (resultado.Succeeded)
                {

                    return RedirectToAction("Index", "Painel");
                }
                ModelState.AddModelError("", _localizador["Verifique seu e-mail e senha"]);
            }
            ViewData["erroLoginOuCadastro"] = 1;
            return View(modelo);
        }


        public IActionResult Registrar()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Registrar(RegistrarViewModel modelo)
        {
            if (ModelState.IsValid)
            {

                var usuario = new UsuarioAplicacao()
                {
                    UserName = modelo.Email,
                    Nome = modelo.Nome,
                    Email = modelo.Email,
                    DataCadastro = DateTime.Now,
                    DataUltimoAcesso = DateTime.Now
                };

                var resultadoCadastro = await _userManager.CreateAsync(usuario, modelo.Senha).ConfigureAwait(false);
                if (resultadoCadastro.Succeeded)
                {
                    await _userManager.AddToRoleAsync(usuario, PerfisPadroes.USUARIO).ConfigureAwait(false);
                    var login = await _signInManager.PasswordSignInAsync(usuario, modelo.Senha, false, lockoutOnFailure: false).ConfigureAwait(false);
                    if (login.Succeeded)
                    {
                        return RedirectToAction("Index", "Painel");
                    }
                }
            }
            ViewData["erroLoginOuCadastro"] = 1;
            ViewData["ApresentarRegrasSenha"] = 1;
            ModelState.AddModelError("", _localizador["Não foi possivel completar o cadastro"]);
            ModelState.AddModelError("", _localizador["Informe todos os campos obrigatórios"]);
            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginExterno(string provedor)
        {
            //var UrlDeRedirecionamento = Url.Action(nameof(RetornoLoginExterno), "Login", new { returnUrl });
            var UrlDeRedirecionamento = Url.Action("RetornoLoginExterno");
            var propriedades = _signInManager.ConfigureExternalAuthenticationProperties(provedor, UrlDeRedirecionamento);
            return Challenge(propriedades, provedor);
        }


        public async Task<IActionResult> RetornoLoginExterno(string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError("", _localizador[$"Ocorreu o seguinte erro com a autetificação externa: {remoteError}"]);
                return RedirectToAction(nameof(Login));
            }


            var informacoesLoginExterno = await _signInManager.GetExternalLoginInfoAsync().ConfigureAwait(false);


            var usuarioExiste = await _userManager.FindByEmailAsync(informacoesLoginExterno.Principal.FindFirstValue(ClaimTypes.Email)).ConfigureAwait(false);


            if (usuarioExiste != null)
            {
                //Chaveamento caso tenha registro local, adicionar login externo
                var existeLoginParaAmarrar = await _userManager.FindByLoginAsync(informacoesLoginExterno.LoginProvider, informacoesLoginExterno.ProviderKey).ConfigureAwait(false);
                if (existeLoginParaAmarrar == null)
                    await _userManager.AddLoginAsync(await _userManager.FindByEmailAsync(informacoesLoginExterno.Principal.FindFirstValue(ClaimTypes.Email)).ConfigureAwait(false), informacoesLoginExterno).ConfigureAwait(false);

                var resultadoLogin = await _signInManager.ExternalLoginSignInAsync(informacoesLoginExterno.LoginProvider, informacoesLoginExterno.ProviderKey, true).ConfigureAwait(false);
                if (resultadoLogin.Succeeded)
                {
                    var TemSenha = await _userManager.HasPasswordAsync(usuarioExiste).ConfigureAwait(false);
                    if (TemSenha)
                    {
                        return RedirectToAction("Index", "Painel");
                    }
                    return RedirectToAction("CriarSenha", "Account");
                }
                return RedirectToAction(nameof(Login));
            }


            var novoUsuario = new UsuarioAplicacao()
            {
                UserName = informacoesLoginExterno.Principal.FindFirstValue(ClaimTypes.Email),
                Email = informacoesLoginExterno.Principal.FindFirstValue(ClaimTypes.Email),
                Nome = informacoesLoginExterno.Principal.FindFirstValue(ClaimTypes.Name),
                DataCadastro = DateTime.Now
            };

            var resultadoDeCadastro = await _userManager.CreateAsync(novoUsuario).ConfigureAwait(false);

            if (resultadoDeCadastro.Succeeded)
            {
                var resultoRel = await _userManager.AddLoginAsync(novoUsuario, informacoesLoginExterno).ConfigureAwait(false);
                await _userManager.AddToRoleAsync(novoUsuario, PerfisPadroes.USUARIO).ConfigureAwait(false);
                var resultadoLogin = await _signInManager.ExternalLoginSignInAsync(informacoesLoginExterno.LoginProvider, informacoesLoginExterno.ProviderKey, true).ConfigureAwait(false);

                if (resultadoLogin.Succeeded & resultoRel.Succeeded)
                {
                    _logger.LogInformation(_localizador["Bem vindo {Name} - Autentificação via "], informacoesLoginExterno.LoginProvider);
                    return RedirectToAction("CriarSenha", "Account");
                }
            }
            return RedirectToAction(nameof(Login));
        }


        [Authorize]
        [HttpGet]
        public IActionResult CriarSenha()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarSenha(CriarSenhaViewModel modelo)
        {
            if (ModelState.IsValid & modelo != null)
            {
                var usuarioLogado = await _userManager.GetUserAsync(User).ConfigureAwait(false);
                var resultadoCricaoSenha = await _userManager.AddPasswordAsync(usuarioLogado, modelo.Senha).ConfigureAwait(false);
                if (resultadoCricaoSenha.Succeeded)
                {
                    return RedirectToAction("Index", "Painel");
                }
            }
            try
            {
                //throw  new FormatException("erro Criar-Senha");
                if (!(modelo.Senha.Equals(modelo.CompararSenha)))
                {
                    ViewData["senhaDif"] = _localizador["As senhas são diferentes"];
                    return View(modelo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Action CriarSenha :: AccountController -> execute: " + ex.ToString());
            }
            ViewData["ApresentarRegrasSenha"] = 1;
            ViewData["erroLoginOuCadastro"] = 1;
            return View(modelo);
        }


        public IActionResult EsqueciASenha()
        {
            //ViewData["Alerta"] = 1;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EsqueciASenha(EsqueciSenhaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(modelo.Email).ConfigureAwait(false);

                var assunto = _localizador["Recuperação de Senha"];
                var msg = _localizador["clique aqui para alterar sua senha"];
                var btn = _localizador["Recuperar Senha"];


                if (usuario != null)
                {
                    var tokenUsuario = await _userManager.GeneratePasswordResetTokenAsync(usuario).ConfigureAwait(false);
                    var urlDeRetorno = Url.Action("RetornoEsqueceuSenha", "Account", new { usuarioId = usuario.Id, token = tokenUsuario }, Request.Scheme);
                    await _emailSender.SendEmailAsync(modelo.Email, assunto, $"{msg}: <a href='{urlDeRetorno}'>{btn}</a>").ConfigureAwait(false);
                }
                ViewData["Alerta"] = 1;
                return View();
            }
            ViewData["erroLoginOuCadastro"] = 1;
            ModelState.AddModelError("", _localizador["É necessário digitar um e-mail"]);
            return View(modelo);
        }

        public IActionResult RetornoEsqueceuSenha(string usuarioId, string token)
        {
            var modelo = new NovaSenhaViewModel()
            {
                UsuarioId = usuarioId,
                Token = token
            };

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> RetornoEsqueceuSenha(NovaSenhaViewModel modelo)
        {
            if (ModelState.IsValid & modelo != null)
            {
                var usuario = await _userManager.FindByIdAsync(modelo.UsuarioId).ConfigureAwait(false);
                if (usuario != null)
                {
                    var resultadoAlteracao = await _userManager.ResetPasswordAsync(usuario, modelo.Token, modelo.NovaSenha).ConfigureAwait(false);
                    if (resultadoAlteracao.Succeeded)
                    {
                        ViewData["Alerta"] = 1;
                        ViewData["Mensagem"] = _localizador["Senha Alterada Com Sucesso"];
                        return View();
                    }
                }
            }
            ModelState.AddModelError("", _localizador["Não conseguimos alterar sua senha!"]);
            ViewData["ApresentarRegrasSenha"] = 1;
            ViewData["erroLoginOuCadastro"] = 1;
            return View(modelo);
        }



        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(false);
            return RedirectToAction("Login", "Account");
        }
    }
}