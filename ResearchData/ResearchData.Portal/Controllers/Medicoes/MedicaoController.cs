using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.Data.Repositorio.Analises;
using ResearchData.Portal.Data.Repositorio.Experimentos;
using ResearchData.Portal.Data.Repositorio.Medicoes;
using ResearchData.Portal.Data.Repositorio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Medicoes;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ResearchData.Portal.Controllers.Medicoes
{
    [Authorize]
    public class MedicaoController : Controller
    {
        private readonly IStringLocalizer<MedicaoController> _localizador;
        private IMedicaoRepositorio _repoMedicao;
        private IAnaliseRepositorio _repoAnalise;
        private IExperimentoRepositorio _repoExperi;
        private ISujeitoRepositorio _repoSujeito;
        private readonly ILogger _logger;

        public MedicaoController(IMedicaoRepositorio medicaoRepositorio, ILogger<MedicaoController> logger, IAnaliseRepositorio analiseRepositorio, IExperimentoRepositorio experimentoRepositorio, ISujeitoRepositorio sujeitoRepositorio, IStringLocalizer<MedicaoController> localizador)
        {
            this._repoMedicao = medicaoRepositorio;
            this._repoAnalise = analiseRepositorio;
            this._repoExperi = experimentoRepositorio;
            this._repoSujeito = sujeitoRepositorio;
            this._localizador = localizador;
            this._logger = logger;
        }



        #region CaptarCaracteriscasComunsGetePost

        public IActionResult CaptarEditarDados(int IdSujeito, int IdAnalise, int IdProjeto)
        {
            AltCadMedicaoViewModel modelo = new AltCadMedicaoViewModel()
            {
                IdSujeito = IdSujeito,
                DescricaoSujeito = _repoSujeito.BuscarDescricaoPorId(IdSujeito),
                IdAnalise = IdAnalise,
                IdProjeto = IdProjeto,
                ListaMedicoes = _repoMedicao.MontarListaModeloMedicoes(IdSujeito)
            };
            return PartialView(modelo);
        }







        [HttpPost]
        public async Task<IActionResult> CaptarEditarDados(AltCadMedicaoViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in modelo.ListaMedicoes)
                {
                    try
                    {
                        //throw  new FormatException("erro Captar-Editar-Dados");
                        if (item.ResultadoMedicao != null && item.ResultadoMedicao != "")
                        {
                            if(item.DataCaptacao == null)
                                item.DataCaptacao = DateTime.Now;
                            await _repoMedicao.SalvarMedicoesComuns(modelo.IdSujeito, modelo.IdAnalise, item);
                        }
                            
                        
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Action CaptarEditarDados :: MedicaoController -> execute: " + ex.ToString());
                        TempData["validacoes"] = _localizador["Não foi possivel realizar as alterações tente novamente"].ToString();
                        return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto});
                    }
                }
                TempData["validacoes"] = _localizador["Alterações realizadas com sucesso"].ToString();
                return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto});
            }
            TempData["validacoes"] = _localizador["Não foi possivel realizar as alterações tente novamente"].ToString();
            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto});
        }

        #endregion


        #region CaptarEditarDadosExperimento


        public async Task<IActionResult> CaptarEditarDadosExperimento(int IdSujeito, int IdAnalise, int IdProjeto, int IdExperimento)
        {
            AltCadMedExperimetoViewModel modelo = new AltCadMedExperimetoViewModel()
            {
                IdSujeito = IdSujeito,
                DescSujeito = _repoSujeito.BuscarDescricaoPorId(IdSujeito),
                IdAnalise = IdAnalise,
                IdProjeto = IdProjeto,
                IdExperimento = IdExperimento,
                NomeExperimento = await _repoExperi.ObterNomeExperimento(IdExperimento),
                ListaMedicoes = _repoMedicao.MontarListaModeloMedicoesExp(IdSujeito, IdExperimento)
            };


            return PartialView(modelo);
        }


        [HttpPost]
        public async Task<IActionResult> CaptarEditarDadosExperimento(AltCadMedExperimetoViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in modelo.ListaMedicoes)
                {
                    try
                    {
                        //throw  new FormatException("erro Captar-Editar-Dados-Experimento");
                        if (item.ResultadoMedicao != null){
                            if(!(item.ResultadoMedicao.ToString().Equals("")))
                                await _repoMedicao.SalvarMedicoesExperimento(modelo.IdSujeito, modelo.IdExperimento, item).ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Action CaptarEditarDadosExperimento :: MedicaoController -> execute: " + ex.ToString());
                        TempData["validacoes"] = _localizador["Não foi possivel realizar as alterações"].ToString();
                        return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto});
                    }
                }
                TempData["validacoes"] = _localizador["Alterações realizadas com sucesso"].ToString();
                return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto});
            }
            TempData["validacoes"] = _localizador["Não foi possivel realizar as alterações"].ToString();
            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto});
        }

        #endregion




        #region AdicionarERemoverExperimentoDeSujeito



        public async Task<IActionResult> AddSujeitoExperimento(int IdAnalise, int IdProjeto, int IdExperimento)
        {
            AltCadMedicaoExpViewModel modelo = new AltCadMedicaoExpViewModel()
            {
                IdAnalise = IdAnalise,
                TituloAnalise = await _repoAnalise.ObterTituloAnalise(IdAnalise),
                IdProjeto = IdProjeto,
                IdExperimento = IdExperimento,
                NomeExp = await _repoExperi.ObterNomeExperimento(IdExperimento),
                SujeitosNoExperimento = _repoMedicao.ObterSujeitosNoExperimento(IdAnalise, IdExperimento),
                SujeitosDisponiveis = _repoMedicao.ObterSujeitosDisponiveisExperimento(IdAnalise, IdExperimento)
            };

            return PartialView(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> AddSujeitoExperimento(AltCadMedicaoExpViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (modelo.SujeitosDisponiveis != null)
                    {
                        var adicionar = modelo.SujeitosDisponiveis.Where(su => su.IsChecked == true);
                        if (adicionar != null)
                        {
                            foreach (var item in adicionar)
                            {
                                await _repoMedicao.AddMedicaoExperimental(modelo.IdExperimento, item.IdSujeito);
                            }
                        }
                    }

                    if (modelo.SujeitosNoExperimento != null)
                    {
                        var remover = modelo.SujeitosNoExperimento.Where(su => su.IsChecked == false);
                        if (remover != null)
                        {
                            foreach (var item in remover)
                            {
                                await _repoMedicao.RemoveMedicaoExperimental(modelo.IdExperimento, item.IdSujeito);
                            }
                        }
                    }
                    TempData["validacoes"] = _localizador["Alterações realizadas com sucesso"].ToString();
                    return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto});
                }
                catch (Exception) { }

            }
            TempData["validacoes"] = _localizador["Ocorreu erro durante o processamento"].ToString();
            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto});
        }

        #endregion




        public async Task<IActionResult> VisualizarMedicaoSujExp(int IdSujeito, int IdExperimento)
        {
            MedicaoExpViewModel modelo = new MedicaoExpViewModel()
            {
                IdSujeito = IdSujeito,
                DescricaoSujeito = _repoSujeito.BuscarDescricaoPorId(IdSujeito),
                IdExperimento = IdExperimento,
                NomeExperimento = await _repoExperi.ObterNomeExperimento(IdExperimento),
                MedicaoVM = _repoSujeito.BuscarMedicaoComunDoSujExperimento(IdSujeito, IdExperimento)
            };

            return PartialView(modelo);
        }



    }

}