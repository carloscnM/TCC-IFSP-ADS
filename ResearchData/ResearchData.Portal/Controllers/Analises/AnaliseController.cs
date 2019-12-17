using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.Data.Repositorio.Analises;
using ResearchData.Portal.Data.Repositorio.Projetos;
using ResearchData.Portal.GerenciamentoUsuario.PerfisDeUsuario;
using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.Models.Negocio.Analises;
using ResearchData.Portal.Models.ViewModels.Analises;
using ResearchData.Portal.Models.ViewModels.Colaborador;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ResearchData.Portal.Data.Repositorio.Colaborador;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ResearchData.Portal.Controllers.Analises
{
    [Authorize(Roles = PerfisPadroes.USUARIO)]
    public class AnaliseController : Controller
    {
        private readonly IStringLocalizer<AnaliseController> _localizador;
        private readonly UserManager<UsuarioAplicacao> _userManager;
        private readonly IAnaliseRepositorio _analiseRepo;
        private readonly IProjetoRepositorio _projetoRepo;
        private readonly ILogger _logger;
        private readonly IColaboradorRepositorio _repoCola;
        private readonly IEmailSender _enviarEmail;

        public AnaliseController(UserManager<UsuarioAplicacao> userManager,
                                    IAnaliseRepositorio AnaliseRepo,
                                        IProjetoRepositorio projetoAnalise,
                                            IStringLocalizer<AnaliseController> localizador,
                                                ILogger<AnaliseController> logger,
                                                 IColaboradorRepositorio colaboradorRepositorio,
                                                 IEmailSender email)
        {
            _userManager = userManager;
            _analiseRepo = AnaliseRepo;
            _projetoRepo = projetoAnalise;
            _localizador = localizador;
            _logger = logger;
            _repoCola = colaboradorRepositorio;
            _enviarEmail = email;
        }

        [HttpGet]
        public IActionResult NovaAnalise(int IdProjeto)
        {
            if (IdProjeto != 0)
            {
                NovaAnaliseViewModel modelo = new NovaAnaliseViewModel()
                {
                    ProjetoId = IdProjeto
                };
                return PartialView(modelo);
            }
            return RedirectToAction("ProjetoEmAndamento", "Projetos");
        }


        [HttpPost]
        public async Task<IActionResult> CadastrarNovaAnalise(NovaAnaliseViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                Analise analise = new Analise()
                {
                    Nome = modelo.Titulo,
                    Descricao = modelo.Descricacao,
                    DateInicio = DateTime.Now,
                    ProjetoId = modelo.ProjetoId
                };

                try
                {
                    //throw  new FormatException("erro cadastro analise");
                    await _analiseRepo.AddAnalise(analise).ConfigureAwait(false);
                    TempData["msg"] = 50;
                    return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.ProjetoId });
                }
                catch (Exception ex)
                {
                    _logger.LogError("Action CadastrarNovaAnalise :: AnaliseController -> execute: " + ex.ToString());
                }
            }
            TempData["msg"] = 51;
            return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.ProjetoId });
        }

        [HttpGet]
        public async Task<IActionResult> AcessarAnalise(int analiseId, int projetoId)
        {
            bool responsavel = _projetoRepo.VerificarResposavel(await ObterIdAsync().ConfigureAwait(false), projetoId);
            bool colaborador = _analiseRepo.VerificarColaborador(await ObterIdAsync().ConfigureAwait(false), analiseId);
            if (responsavel || colaborador)
            {
                if (analiseId != 0)
                {
                    var analise = await _analiseRepo.BuscarAnalise(analiseId).ConfigureAwait(false);
                    var tituloProjeto = await _projetoRepo.ObterTituloProjeto(projetoId).ConfigureAwait(false);
                    var responsaveNome = _projetoRepo.ObterResponsavel(projetoId);
                    bool TemSujeito = _analiseRepo.ExisteSujeito(analiseId);

                    AcessoAnaliseViewModel modelo = new AcessoAnaliseViewModel()
                    {
                        IdAnalise = analise.Id,
                        projetoId = analise.ProjetoId,
                        Titulo = analise.Nome,
                        Descricacao = analise.Descricao,
                        Criacao = analise.DateInicio,
                        Encerramento = analise.DataFim,
                        TituloProjeto = tituloProjeto,
                        TemSujeito = TemSujeito,
                        Responsavel = responsaveNome
                    };

                    if (responsavel)
                    {
                        modelo.Acesso = 2;
                    }
                    else
                    {
                        modelo.Acesso = _analiseRepo.ConsultarAcesso(await ObterIdAsync().ConfigureAwait(false));
                    }





                    if (TempData["ResultadoEncAnalise"] != null)
                        ViewBag.MsgTelaAna = TempData["ResultadoEncAnalise"];

                    if (TempData["validacoes"] != null)
                        ViewBag.MsgTelaAna = TempData["validacoes"];


                    return View(modelo);
                }
                return RedirectToAction("AcessarProjeto", "Projetos", new { Id = projetoId });
            }
            return StatusCode(203);
        }


        public IActionResult AdicionarColaborador(int IdAnalise, int IdProjeto)
        {
            NovoColaboradorViewModel modelo = new NovoColaboradorViewModel()
            {
                IdAnalise = IdAnalise,
                IdProjeto = IdProjeto
            };
            return PartialView(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarColaborador(NovoColaboradorViewModel modelo)
        {
            int msg = 2;
            var usuarioId = await ObterIdAsync().ConfigureAwait(false);

            bool resultado = _analiseRepo.AdicionarColaborador(modelo.IdAnalise, modelo.Email, usuarioId, modelo.IdProjeto, modelo.Acesso);

            if (resultado)
            {
                var titulo = await _analiseRepo.ObterTituloAnalise(modelo.IdAnalise).ConfigureAwait(false);
                var reposavel = _projetoRepo.ObterResponsavel(modelo.IdProjeto);
                var reposavelNome = reposavel.Nome;


                reposavelNome = reposavelNome = reposavelNome.Substring(0, reposavelNome.IndexOf(" "));
                string colaborador = _repoCola.ObterNomeColaborador(modelo.Email);


                string mensagem = $"{colaborador}, " + _localizador["Agora você é um novo Colaborador ! Acesse"].ToString() + "<a href='https://rdfacility.mindsecurity.org/'>Research Data Facility</a>, " +
                    $"" + _localizador["e comece a coletar dados!"].ToString() + " <br />" +
                    $"<b>" + _localizador["Análise"].ToString() + ": </b> {titulo} <br /> <b>" + _localizador["Responsável"].ToString() + " </b>{reposavelNome}";


                var assunto = _localizador["Nova analise na sua lista!"].ToString();

                await _enviarEmail.SendEmailAsync(modelo.Email, assunto, mensagem).ConfigureAwait(false);
                msg = 1;
            }

            TempData["msg"] = msg;
            return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.IdProjeto });
        }

        //NovaAnaliseViewModel

        public async Task<IActionResult> EncerrarAnalise(int IdAnalise, int IdProjeto)
        {
            var Autorizado = _projetoRepo.VerificarResposavel(await ObterIdAsync().ConfigureAwait(false), IdProjeto);
            var CorAutorizado = _analiseRepo.VerificarColaborador(await ObterIdAsync().ConfigureAwait(false), IdAnalise);
            if (Autorizado || CorAutorizado)
            {
                var analise = await _analiseRepo.BuscarAnalise(IdAnalise).ConfigureAwait(false);
                NovaAnaliseViewModel modelo = new NovaAnaliseViewModel()
                {
                    IdAnalise = analise.Id,
                    ProjetoId = analise.ProjetoId,
                    Titulo = analise.Nome
                };
                return PartialView(modelo);
            }
            return StatusCode(203);
        }


        [HttpPost]
        public async Task<IActionResult> EncerrarAnalise(NovaAnaliseViewModel modelo)
        {
            try
            {
                var Autorizado = _projetoRepo.VerificarResposavel(await ObterIdAsync().ConfigureAwait(false), modelo.ProjetoId);
                var CorAutorizado = _analiseRepo.VerificarColaborador(await ObterIdAsync().ConfigureAwait(false), modelo.IdAnalise);

                if (Autorizado || CorAutorizado)
                {
                    await _analiseRepo.EncerraAnalise(modelo.IdAnalise).ConfigureAwait(false);

                    TempData["ResultadoEncAnalise"] = _localizador["Análise encerrada com sucesso"].ToString();
                }
                else
                {
                    //não traduzir....
                    new Exception("Não Autorizado para encerrar Análise");
                }
            }
            catch (Exception ex)
            {
                TempData["ResultadoEncAnalise"] = _localizador["Não foi possivel encerrar a análise"].ToString();
                //não traduzir loogger...
                _logger.LogError("Action :: EncerrarAnalise :: AnaliseController -> Execute:" + ex.Message);
            }

            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.ProjetoId });
        }

        public async Task<IActionResult> RemoverAnalise(int IdAnalise, int IdProjeto)
        {
            bool responsavel = _projetoRepo.VerificarResposavel(await ObterIdAsync().ConfigureAwait(false), IdProjeto);
            if (responsavel)
            {
                var analise = await _analiseRepo.BuscarAnalise(IdAnalise).ConfigureAwait(false);
                NovaAnaliseViewModel modelo = new NovaAnaliseViewModel()
                {
                    IdAnalise = analise.Id,
                    Titulo = analise.Nome,
                    ProjetoId = analise.ProjetoId
                };
                return PartialView(modelo);
            }
            return StatusCode(203);
        }

        [HttpPost]
        public async Task<IActionResult> RemoverAnalise(NovaAnaliseViewModel modelo)
        {
            bool responsavel = _projetoRepo.VerificarResposavel(await ObterIdAsync().ConfigureAwait(false), modelo.ProjetoId);
            if (responsavel)
            {
                Analise analise = await _analiseRepo.BuscarAnalise(modelo.IdAnalise).ConfigureAwait(false);
                analise.Ativa = false;
                bool resultado = _analiseRepo.AlterarAnalise(analise);
                if (resultado)
                {
                    await _repoCola.RemoverColaboradores(modelo.IdAnalise);
                    TempData["msg"] = 10;
                    return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.ProjetoId });
                }
                TempData["msg"] = 11;
                return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.ProjetoId });
            }
            return StatusCode(203);
        }

        public async Task<IActionResult> EditarAnalise(int IdAnalise, int IdProjeto)
        {
            bool responsavel = _projetoRepo.VerificarResposavel(await ObterIdAsync().ConfigureAwait(false), IdProjeto);
            if (responsavel)
            {
                var analise = await _analiseRepo.BuscarAnalise(IdAnalise);
                NovaAnaliseViewModel modelo = new NovaAnaliseViewModel()
                {
                    IdAnalise = analise.Id,
                    Titulo = analise.Nome,
                    Descricacao = analise.Descricao,
                    ProjetoId = analise.ProjetoId
                };
                return PartialView(modelo);
            }
            return StatusCode(203);
        }

        [HttpPost]
        public async Task<IActionResult> EditarAnalise(NovaAnaliseViewModel modelo)
        {
            bool responsavel = _projetoRepo.VerificarResposavel(await ObterIdAsync().ConfigureAwait(false), modelo.ProjetoId);
            if (responsavel)
            {
                Analise analise = await _analiseRepo.BuscarAnalise(modelo.IdAnalise).ConfigureAwait(false);
                analise.Nome = modelo.Titulo;
                analise.Descricao = modelo.Descricacao;
                bool resultado = _analiseRepo.AlterarAnalise(analise);
                if (resultado)
                {
                    TempData["msg"] = 12;
                    return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.ProjetoId });
                }
                TempData["msg"] = 11;
                return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.ProjetoId });
            }
            return StatusCode(203);
        }

        public async Task<IActionResult> AbrirAnalise(int IdAnalise, int IdProjeto)
        {
            bool responsavel = _projetoRepo.VerificarResposavel(await ObterIdAsync().ConfigureAwait(false), IdProjeto);
            if (responsavel)
            {
                var analise = await _analiseRepo.BuscarAnalise(IdAnalise).ConfigureAwait(false);
                NovaAnaliseViewModel modelo = new NovaAnaliseViewModel()
                {
                    IdAnalise = analise.Id,
                    Titulo = analise.Nome,
                    Descricacao = analise.Descricao,
                    ProjetoId = analise.ProjetoId
                };
                return PartialView(modelo);
            }
            return StatusCode(203);
        }

        [HttpPost]
        public async Task<IActionResult> AbrirAnalise(NovaAnaliseViewModel modelo)
        {
            bool responsavel = _projetoRepo.VerificarResposavel(await ObterIdAsync().ConfigureAwait(false), modelo.ProjetoId);
            if (responsavel)
            {
                Analise analise = await _analiseRepo.BuscarAnalise(modelo.IdAnalise).ConfigureAwait(false);
                analise.DataFim = null;
                bool resultado = _analiseRepo.AlterarAnalise(analise);
                if (resultado)
                {
                    TempData["msg"] = 30;
                    return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.ProjetoId });
                }
                TempData["msg"] = 31;
                return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.ProjetoId });
            }
            return StatusCode(203);
        }





        private async Task<string> ObterIdAsync()
        {
            var usuario = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            return Convert.ToString(usuario.Id);
        }

    }
}