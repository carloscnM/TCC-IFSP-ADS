using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ResearchData.Portal.Models.ViewModels.Relatorio;
using Microsoft.AspNetCore.Identity;
using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.Data.Repositorio.Analises;
using ResearchData.Portal.Data.Repositorio.Projetos;
using Microsoft.Extensions.Logging;
using ResearchData.Portal.Controllers.Analises;
using ResearchData.Portal.Models.Exportacao;
using System.IO;
using System.Text;
using ResearchData.Portal.Data.Repositorio.Grupos;

namespace ResearchData.Portal.Controllers.Relatorios
{
    [Authorize]
    public class RelatorioController : Controller
    {
        private readonly IStringLocalizer<RelatorioController> _localizador;
        private UserManager<UsuarioAplicacao> _userManager;
        private IAnaliseRepositorio _repoAnalise;
        private IProjetoRepositorio _repoProjeto;
        private ILogger<RelatorioController> _logger;
        private IExportarCsv _montarCsv;
        private IGrupoRepositorio _grupoRepo;

        public RelatorioController(IStringLocalizer<RelatorioController> localizador,
                                         UserManager<UsuarioAplicacao> userManager,
                                                IAnaliseRepositorio AnaliseRepo,
                                                    IProjetoRepositorio projetoAnalise,
                                                        ILogger<RelatorioController> logger,
                                                        IExportarCsv relatorioCsv,
                                                            IGrupoRepositorio grupoRepo)
        {
            _localizador = localizador;
            _userManager = userManager;
            _repoAnalise = AnaliseRepo;
            _repoProjeto = projetoAnalise;
            _logger = logger;
            _montarCsv = relatorioCsv;
            _grupoRepo = grupoRepo;
        }

        public IActionResult RelatorioDeProjeto()
        {
            return View();
        }

        public async Task<IActionResult> RelatorioAnalise(int IdAnalise, int IdProjeto)
        {
            bool responsavel = _repoProjeto.VerificarResposavel(await ObterIdAsync(), IdProjeto);
            bool colaborador = _repoAnalise.VerificarColaborador(await ObterIdAsync(), IdAnalise);

            if (colaborador || responsavel)
            {
                try
                {
                    RelatorioDeAnaliseViewModel modelo = new RelatorioDeAnaliseViewModel()
                    {
                        DetalheProjeto = await _repoProjeto.DetalhesProjeto(IdProjeto),
                        DetalheAnalise = await _repoAnalise.DetalhesAnalise(IdAnalise),
                        Metricas = _repoAnalise.DetalheMetricaAnalise(IdAnalise),
                        SujeitosPorExperimento = _repoAnalise.RelacaoSujeitosPorExperimento(IdAnalise),
                        SujeitosPorgrupo = _grupoRepo.ObterQtdSujeitosPorGrupo(IdAnalise)
                    };
                    return View(modelo);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao executar solicitação de relatório Projeto {IdProjeto}, Análise {IdAnalise} : execute : {ex.Message}");
                    throw;
                }
            }
            return StatusCode(203);
        }


        [HttpGet]
        public async Task<IActionResult> RelatorioDeAnaliseAnaliticoCsv(int IdAnalise, int IdProjeto)
        {
            bool responsavel = _repoProjeto.VerificarResposavel(await ObterIdAsync(), IdProjeto);
            bool colaborador = _repoAnalise.VerificarColaborador(await ObterIdAsync(), IdAnalise);

            if (colaborador || responsavel)
            {

                //Gerando CSV
                var dados = _montarCsv.RetornarDadosAnaliticoAnalise(IdAnalise);

                var stream = new MemoryStream(Encoding.Unicode.GetBytes(dados));
                var resultado = new FileStreamResult(stream, "text/plain");
                resultado.FileDownloadName = "Analise_Csv_" + DateTime.Now + ".csv";
                //FimGeracaoCsv
                return resultado;
            }

            return StatusCode(203);
        }


        private async Task<string> ObterIdAsync()
        {
            var usuario = await _userManager.GetUserAsync(User).ConfigureAwait(true);
            return Convert.ToString(usuario.Id);
        }



    }
}