using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.Data.Repositorio.Analises;
using ResearchData.Portal.Data.Repositorio.Projetos;
using ResearchData.Portal.Models.Projetos.Negocio;
using ResearchData.Portal.Models.ViewModels.Analises;
using ResearchData.Portal.Models.ViewModels.Projetos;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.GerenciamentoUsuario.PerfisDeUsuario;

namespace ResearchData.Portal.Controllers.Projetos
{
    [Authorize(Roles = PerfisPadroes.USUARIO)]
    public class ProjetosController : Controller
    {
        private readonly IStringLocalizer<ProjetosController> _localizador;
        private readonly UserManager<UsuarioAplicacao> _userManager;
        private readonly IProjetoRepositorio _projetoRepo;
        private readonly IAnaliseRepositorio _analiseRepo;
        private readonly ILogger _logger;


        public ProjetosController(UserManager<UsuarioAplicacao> userManager, ILogger<ProjetosController> logger, IProjetoRepositorio projetoRepo, IAnaliseRepositorio AnaliseRepo, IStringLocalizer<ProjetosController> localizador)
        {
            _userManager = userManager;
            _projetoRepo = projetoRepo;
            _analiseRepo = AnaliseRepo;
            _localizador = localizador;
            _logger = logger;
        }


        public IActionResult CriarProjeto()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarAlterarProjeto(ProjetoViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                Projeto projeto = new Projeto()
                {
                    Id = modelo.Id,
                    Titulo = modelo.Titulo,
                    Descricao = modelo.Descricacao,
                    Ativo = true,
                    UsuarioAplicacaoId = await ObterIdAsync()
                };
                try
                {
                    if (projeto.Id != 0)
                    {
                        var resultado = await _projetoRepo.AlterarProjeto(projeto);
                        if (resultado)
                        {
                            TempData["valProjetos"] = _localizador["Projeto Alterado com sucesso"].ToString();
                            return RedirectToAction("ProjetoEmAndamento", "Projetos");
                        }
                        new Exception();
                    }
                    else
                    {
                        projeto.DataInicial = DateTime.Now;
                        await _projetoRepo.AddProjeto(projeto);
                        var msg = _localizador["Projeto Cadastrado com sucesso"].ToString();
                        TempData["valProjetos"] = msg.ToString();
                        return RedirectToAction("ProjetoEmAndamento", "Projetos");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Action CriarAlterarProjeto :: ProjetoController -> execute: " + ex.ToString());
                }
            }
            TempData["valProjetos"] = _localizador["Ocorreu erro durante o processamento"].ToString();
            return RedirectToAction("ProjetoEmAndamento", "Projetos");
        }


        public IActionResult ProjetoEmAndamento()
        {

            if (TempData["valProjetos"] != null)
                ViewData["alerta"] = TempData["valProjetos"];

            return View();
        }

        public async Task<IActionResult> AcessarProjeto(int Id, int? msgSucessErro)
        {
            bool responsavel = _projetoRepo.VerificarResposavel(await ObterIdAsync().ConfigureAwait(false), Id);
            if (responsavel)
            {


                if (Id != 0)
                {
                    var idUsuario = Convert.ToString(await ObterIdAsync());
                    var projeto = await _projetoRepo.BuscarProjetoDoUsuario(Id, idUsuario);
                    var analise = _analiseRepo.AnalisePorProjeto(Id);
                    bool analiseAberta = false;

                    if (analise != null)
                    {
                        var qtdAnaliseAberta = analise.Where(a => a.DataFim == null).Count();

                        if (qtdAnaliseAberta > 0)
                            analiseAberta = true;
                    }



                    if (projeto != null)
                    {
                        AcessoProjetoViewModel modelo = new AcessoProjetoViewModel()
                        {
                            Id = projeto.Id.ToString(),
                            Titulo = projeto.Titulo,
                            Descricacao = projeto.Descricao,
                            DataCriacao = projeto.DataInicial.ToString().Substring(0, projeto.DataInicial.ToString().IndexOf(" ")),
                            DataFinal = projeto.DataFinal,
                            AnalisesAberta = analiseAberta,
                            ListaAnalise = analise
                        };

                        #region MensagensValidacao


                        if (TempData["msg"] != null)
                        {
                            ViewData["MsgAcaoTela"] = ObterMensagemDeAcao(Convert.ToInt16(TempData["msg"].ToString()));
                        }
                        #endregion

                        return View(modelo);
                    }
                }
                return RedirectToAction(nameof(ProjetoEmAndamento));
            }
            return StatusCode(203);
        }

        #region MsgSucesso

        private string ObterMensagemDeAcao(int msg)
        {
            switch (msg)
            {
                case 1:
                    return _localizador["Colaborador Cadastrado com sucesso"].ToString();
                case 2:
                    return _localizador["Não foi Possivel completar a ação"].ToString();
                case 3:
                    return _localizador["Colaborador removido da análise com sucesso"].ToString();
                case 10:
                    return _localizador["Analise removida com sucesso"].ToString();
                case 11:
                    return _localizador["Não foi possivel realizar ação na análise"].ToString();
                case 12:
                    return _localizador["Análise alterada com sucesso"].ToString();
                case 20:
                    return _localizador["Projeto encerrado com sucesso"].ToString();
                case 21:
                    return _localizador["Não foi possivel encerrar o projeto"].ToString();
                case 30:
                    return _localizador["Abertura de analise realizada com sucesso"].ToString();
                case 31:
                    return _localizador["Não foi possivel realizar abertura da analise"].ToString();
                case 40:
                    return _localizador["Abertura de Projeto Realizada com sucesso"].ToString();
                case 41:
                    return _localizador["Não foi possivel realizar abertura do Projeto"].ToString();
                case 50:
                    return _localizador["Análise Cadastrada com sucesso"].ToString();
                case 51:
                    return _localizador["Não foi possivel cadastrar a análise"].ToString();
                default:
                    return _localizador["Tente Novamente mais tarde"].ToString();
            }
        }

        #endregion


        public async Task<IActionResult> EditarProjeto(int Id)
        {
            var projeto = await _projetoRepo.BuscarProjetoPorId(Id);

            ProjetoViewModel modelo = new ProjetoViewModel()
            {
                Id = projeto.Id,
                Titulo = projeto.Titulo,
                Descricacao = projeto.Descricao
            };

            return PartialView(modelo);
        }

        public async Task<IActionResult> ExcluirProjeto(int Id)
        {
            var projeto = await _projetoRepo.BuscarProjetoPorId(Id);

            ProjetoViewModel modelo = new ProjetoViewModel()
            {
                Id = projeto.Id,
                Titulo = projeto.Titulo,
                Descricacao = projeto.Descricao
            };

            return PartialView(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirProjeto(ProjetoViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                Projeto projeto = new Projeto()
                {
                    Id = modelo.Id,
                    Titulo = modelo.Titulo,
                    Descricao = modelo.Descricacao,
                    Ativo = false,
                    UsuarioAplicacaoId = await ObterIdAsync(),
                };
                try
                {
                    //throw  new FormatException("erro Excluir-Projeto");
                    await _projetoRepo.ExcluirProjeto(projeto);
                    await _analiseRepo.DevativarAnalisesDoProjeto(projeto.Id);

                    TempData["valProjetos"] = _localizador["Projeto removido com sucesso"].ToString();
                    return RedirectToAction("ProjetoEmAndamento", "Projetos");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Action ExcluirProjeto :: ProjetoController -> execute: " + ex.ToString());

                }
            }
            TempData["valProjetos"] = _localizador["Não foi possível remover o projeto"].ToString();
            return RedirectToAction("ProjetoEmAndamento", "Projetos");
        }


        public async Task<IActionResult> ColaboradoresDaAnalise(int IdAnalise)
        {
            var idProejto = await _projetoRepo.BuscarIdProjetoPorAnalise(IdAnalise);

            ColaboradorAnaliseViewModel modelo = new ColaboradorAnaliseViewModel()
            {
                IdAnalise = IdAnalise,
                IdProjeto = idProejto,
                Lista = _analiseRepo.BuscarColaboradoresDaAnalise(IdAnalise)
            };

            return PartialView(modelo);
        }


        public async Task<IActionResult> EncerrarProjeto(int IdProjeto)
        {
            bool responsavel = _projetoRepo.VerificarResposavel(await ObterIdAsync(), IdProjeto);
            if (responsavel)
            {
                DadosSimplesProjetoViewModel modelo = _projetoRepo.ObterDetalheProjetoSimplificado(IdProjeto);
                return PartialView(modelo);
            }
            return StatusCode(203);
        }

        [HttpPost]
        public async Task<IActionResult> EncerrarProjeto(DadosSimplesProjetoViewModel modelo)
        {
            bool responsavel = _projetoRepo.VerificarResposavel(await ObterIdAsync(), modelo.IdProjeto);
            if (responsavel)
            {
                try
                {
                    Projeto projeto = await _projetoRepo.BuscarProjetoPorId(modelo.IdProjeto);
                    projeto.DataFinal = DateTime.Now;
                    var resultado = await _projetoRepo.AlterarProjeto(projeto);
                    if (resultado)
                    {
                        TempData["msg"] = 20;
                        return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.IdProjeto });
                    }
                }
                catch (Exception)
                {
                    //gravar o Log
                }
                TempData["msg"] = 21;
                return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.IdProjeto });
            }
            return StatusCode(203);
        }

        public async Task<IActionResult> AbrirProjeto(int IdProjeto)
        {
            bool responsavel = _projetoRepo.VerificarResposavel(await ObterIdAsync(), IdProjeto);
            if (responsavel)
            {
                DadosSimplesProjetoViewModel modelo = _projetoRepo.ObterDetalheProjetoSimplificado(IdProjeto);
                return PartialView(modelo);
            }
            return StatusCode(203);
        }


        [HttpPost]
        public async Task<IActionResult> AbrirProjeto(DadosSimplesProjetoViewModel modelo)
        {
            bool responsavel = _projetoRepo.VerificarResposavel(await ObterIdAsync(), modelo.IdProjeto);
            if (responsavel)
            {
                try
                {
                    Projeto projeto = await _projetoRepo.BuscarProjetoPorId(modelo.IdProjeto);
                    projeto.DataFinal = null;
                    var resultado = await _projetoRepo.AlterarProjeto(projeto);
                    if (resultado)
                    {
                        TempData["msg"] = 40;
                        return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.IdProjeto });
                    }
                }
                catch (Exception)
                {
                    //gravar o Log
                }
                TempData["msg"] = 41;
                return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.IdProjeto });
            }
            return StatusCode(203);
        }


        public async Task<IActionResult> AdicionarColaborador(int IdAnalise)
        {
            int idprojeto = await _projetoRepo.BuscarIdProjetoPorAnalise(IdAnalise);
            return RedirectToAction("AdicionarColaborador", "Analise", new { IdAnalise = IdAnalise, idProjeto = idprojeto });
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