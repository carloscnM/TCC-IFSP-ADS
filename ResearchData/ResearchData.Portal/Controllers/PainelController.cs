using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.GerenciamentoUsuario;
using Microsoft.Extensions.Localization;
using ResearchData.Portal.Data.Repositorio.Projetos;
using ResearchData.Portal.Models.ViewModels.Painel;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ResearchData.Portal.Controllers
{
    [Authorize]
    public class PainelController : Controller
    {
        private readonly IStringLocalizer<AccountController> _localizador;
        private IProjetoRepositorio _projetoRepo;
        private IHttpContextAccessor _contextoApp;
        private ILogger _logger;
        private readonly UserManager<UsuarioAplicacao> _userManager;

        public PainelController(UserManager<UsuarioAplicacao> userManager, IStringLocalizer<AccountController> localizador,
                                                            IProjetoRepositorio projetoRepositorio,
                                                                IHttpContextAccessor contextoApp,
                                                                    ILogger<PainelController> logger)
        {
            _userManager = userManager;
            _localizador = localizador;
            _projetoRepo = projetoRepositorio;
            _contextoApp = contextoApp;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var ipUsuario = _contextoApp.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            await RegistrarAcesso(ipUsuario);

            var usuarioId = await ObterIdAsync();

            PainelViewModel modelo = new PainelViewModel()
            {
                ProjetosRecente = _projetoRepo.ObterProjetosRecente(usuarioId, 2),
                QuantidadeProjetoAnd = _projetoRepo.ObterQuantidadeProjetoAndamento(usuarioId, true),
                QuantidadeProjetoEnc = _projetoRepo.ObterQuantidadeProjetoAndamento(usuarioId, false),
                QuantidadeColaboracoes = _projetoRepo.ObterQuantidadeColaboracoes(usuarioId)
            };

            return View(modelo);
        }


        private async Task RegistrarAcesso(string ip)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
                user.DataUltimoAcesso = DateTime.Now;

                _logger.LogError($"Usuario-Email: {user.Email} acessou em: {DateTime.Now} a apalicação reseachDataFacility com endereço remoto {ip}");

                await _userManager.UpdateAsync(user).ConfigureAwait(false);
            }
        }

        private async Task<string> ObterIdAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var usuario = await _userManager.GetUserAsync(User);
                return Convert.ToString(usuario.Id);
            }
            return "";
        }
    }
}