using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.Data.Repositorio.Analises;
using ResearchData.Portal.Data.Repositorio.Caracteristicas;
using ResearchData.Portal.Data.Repositorio.Grupos;
using ResearchData.Portal.Data.Repositorio.Sujeitos;
using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Sujeitos;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ResearchData.Portal.Controllers.Sujeitos
{
    public class SujeitoController : Controller
    {
        #region RepositoriosERecursosDisponiveis

        private ISujeitoRepositorio _sujeitoRepo;
        private ICaracteristicaRepositorio _caraRepo;
        private IAnaliseRepositorio _analiseRepo;
        private IGrupoRepositorio _grupoRepo;
        private readonly IStringLocalizer<SujeitoController> _localizador;
        private readonly ILogger<SujeitoController> _logger;

        #endregion


        #region ContrutorInjecaoDep
        public SujeitoController(IStringLocalizer<SujeitoController> localizador, ILogger<SujeitoController> logger, ISujeitoRepositorio sujeitoRepositorio, ICaracteristicaRepositorio caracteristicaRepositorio, IAnaliseRepositorio analiseRepositorio, IGrupoRepositorio grupoRepositorio)
        {
            this._sujeitoRepo = sujeitoRepositorio;
            this._caraRepo = caracteristicaRepositorio;
            this._analiseRepo = analiseRepositorio;
            this._grupoRepo = grupoRepositorio;
            this._localizador = localizador;
            this._logger = logger;
        }

        #endregion

        #region CadastroDeCaracteristicasComunsParaSujeito

        public IActionResult CaracteristicasComuns(int IdSujeito)
        {
            VisualizarCaracComunsViewModel modelo = new VisualizarCaracComunsViewModel()
            {
                IdSujeito = IdSujeito,
                DescricaoSujeito = _sujeitoRepo.BuscarDescricaoPorId(IdSujeito),
                MedicaoVM = _sujeitoRepo.BuscarMedicaoComunDoSujeito(IdSujeito)
            };

            return PartialView(modelo);
        }


        public async Task<IActionResult> CadastrarCaraSujeito(int IdAnalise, int IdProjeto)
        {
            SelecionarSujeitosViewModel modelo = new SelecionarSujeitosViewModel()
            {
                IdAnalise = IdAnalise,
                TituloAnalise = await _analiseRepo.ObterTituloAnalise(IdAnalise),
                IdProjeto = IdProjeto,
                ListaCaracteristicaSelecionadas = _caraRepo.ListarCaracteriscasSelecionadas(IdAnalise),
                ListaCaracteristicaNSelecionadas = _caraRepo.ListaCaracteristicaDisponiveis(IdAnalise)

            };

            return PartialView(modelo);
        }


        [HttpPost]
        public async Task<IActionResult> CadastrarCaraSujeito(SelecionarSujeitosViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (modelo.ListaCaracteristicaSelecionadas != null)
                    {
                        var removidas = modelo.ListaCaracteristicaSelecionadas.Where(car => car.IsChecked == false);
                        foreach (var item in removidas)
                        {
                            try
                            {
                                await _caraRepo.RemoverCaracteristicaComunAnalise(modelo.IdAnalise, item.CaracteristicaId).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError("Action CadastrarCaraSujeito.Removidas :: SujeitoController -> execute: " + ex.ToString());
                            }
                        }
                    }
                    if (modelo.ListaCaracteristicaNSelecionadas != null)
                    {
                        var adicionar = modelo.ListaCaracteristicaNSelecionadas.Where(car => car.IsChecked == true);

                        if (adicionar != null)
                        {
                            foreach (var item in adicionar)
                            {
                                try
                                {
                                    await _caraRepo.AdicionarCaracteriticaAnalise(modelo.IdAnalise, item.CaracteristicaId).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError("Action CadastrarNovaAnalise.Removidas :: SujeitoController -> execute: " + ex.ToString());
                                }
                            }
                        }
                    }
                    TempData["validacoes"] = _localizador["Alterações realizadas com sucesso"].ToString();
                    return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto });
                }
                catch (Exception)
                {

                }
            }
            TempData["validacoes"] = _localizador["Erro durante o processamente, tente novamente"].ToString();
            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto });
        }

        #endregion



        #region NovoSujeitoParaAnalise

        public async Task<IActionResult> NovoSujeito(int IdAnalise, int IdProjeto)
        {
            NovoSujeitoViewModel modelo = new NovoSujeitoViewModel()
            {
                AnaliseId = IdAnalise,
                ProjetoId = IdProjeto,
                Grupos = await _grupoRepo.BuscarGrupoPorProjeto(IdProjeto)
            };
            return PartialView(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> NovoSujeito(NovoSujeitoViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                SujeitoExperimental sujeito = new SujeitoExperimental()
                {
                    Descricacao = modelo.Descricao,
                    AnaliseId = modelo.AnaliseId,
                    GrupoId = modelo.GrupoId
                };

                try
                {
                    //throw  new FormatException("erro Novo-Sujeito");
                    var resultado = await _sujeitoRepo.CadastrarSujeitoAnalise(sujeito).ConfigureAwait(false);
                    if (resultado)
                    {
                        TempData["validacoes"] = _localizador["Sujeito Cadastrado com sucesso"].ToString();
                        return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId });
                    }
                    TempData["validacoes"] = _localizador["Não foi possivel realizar o cadastro do sujeito"].ToString();
                    return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId });
                }
                catch (Exception ex)
                {
                    _logger.LogError("Action NovoSujeito :: SujeitoController -> execute: " + ex.ToString());
                }
            }
            TempData["validacoes"] = _localizador["Não foi possivel realizar o cadastro do sujeito"].ToString();
            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId });
        }


        #endregion


        public IActionResult ListarExperimentoSujeito(int IdAnalise, int IdProjeto, int IdSujeito)
        {
            ExpDoSujeitoViewModel modelo = new ExpDoSujeitoViewModel()
            {
                IdAnalise = IdAnalise,
                IdProjeto = IdProjeto,
                IdSujeito = IdSujeito,
                AnaliseAtiva = _sujeitoRepo.VerificarAnaliseAtiva(IdAnalise),
                NomeSujeito = _sujeitoRepo.BuscarDescricaoPorId(IdSujeito),
                ListaExp = _sujeitoRepo.BuscarExpDoSujeito(IdSujeito)
            };

            return PartialView(modelo);
        }

        #region EditarOuRemoverSujeito

        public async Task<IActionResult> EditarSujeito(int IdSujeito, int IdAnalise, int IdProjeto)
        {
            NovoSujeitoViewModel modelo = _sujeitoRepo.ObterDetalheSujeito(IdSujeito);
            if (modelo != null)
            {
                modelo.ProjetoId = IdProjeto;
                modelo.Grupos = await _grupoRepo.BuscarGrupoPorProjeto(IdProjeto);
                return PartialView(modelo);
            }
            TempData["validacoes"] = _localizador["Erro durante o carregamento dos dados"].ToString();
            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = IdAnalise, projetoId = IdProjeto });
        }

        [HttpPost]
        public async Task<IActionResult> EditarSujeito(NovoSujeitoViewModel modelo)
        {
            try
            {
                bool resultado = await _sujeitoRepo.EditarSujeito(modelo);
                if (resultado)
                {
                    TempData["validacoes"] = _localizador["Sujeito Alterado com sucesso"].ToString();
                    return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId });
                }
                new InvalidOperationException();
            }
            catch (Exception)
            {
                //gravar log..
            }
            TempData["validacoes"] = _localizador["Não foi possivel realizar a alteração"].ToString();
            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId });
        }

        public IActionResult RemoverSujeito(int IdSujeito, int IdAnalise, int IdProjeto)
        {
            NovoSujeitoViewModel modelo = _sujeitoRepo.ObterDetalheSujeito(IdSujeito);
            if (modelo != null)
            {
                modelo.ProjetoId = IdProjeto;
                return PartialView(modelo);
            }
            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = IdAnalise, projetoId = IdProjeto });
        }

        [HttpPost]
        public async Task<IActionResult> RemoverSujeito(NovoSujeitoViewModel modelo)
        {
            try
            {
                bool resultado = await _sujeitoRepo.RemoverSujeito(modelo);
                if (resultado)
                {
                    TempData["validacoes"] = _localizador["Sujeito Removido com sucesso"].ToString();
                    return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId });
                }
                new InvalidOperationException();
            }
            catch (Exception)
            {
                //gravar log..
            }
            TempData["validacoes"] = _localizador["Nao foi possivel remover o sujeito"].ToString();
            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId });
        }

        #endregion


        public async Task<IActionResult> ReutilizacaoSujeito(int idAnalise, int IdProjeto)
        {
            SujeitoExistenteViewModel modelo = new SujeitoExistenteViewModel()
            {
                IdAnalise = idAnalise,
                IdProjeto = IdProjeto,
                Grupos = await _grupoRepo.BuscarGrupoPorProjeto(IdProjeto).ConfigureAwait(false),
                ListaSujeito = _sujeitoRepo.BuscarSujeitoExistente(idAnalise, IdProjeto)
            };

            return PartialView(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> ReutilizacaoSujeito(SujeitoExistenteViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var selecionados = modelo.ListaSujeito.Where(l => l.IsChecked == true);

                    if(selecionados.Count() > 0)
                    {
                        IList<bool> resultado = new List<bool>();
                        foreach (var item in selecionados)
                        {
                            SujeitoExperimental sujeito = new SujeitoExperimental()
                            {
                                Descricacao = item.Descricao,
                                AnaliseId = modelo.IdAnalise,
                                GrupoId = modelo.GrupoId,
                                Externo = item.IdSujeito
                            };

                            //throw  new FormatException("erro Novo-Sujeito");
                            resultado.Add(await _sujeitoRepo.CadastrarSujeitoAnalise(sujeito).ConfigureAwait(false));
                        }
                        if (resultado.Where(r => r == true).Any())
                        {
                            TempData["validacoes"] = "Um ou mais sujeitos foram adicionados com sucesso!";
                            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto });
                        }

                    }
                    else
                    {
                        TempData["validacoes"] = "Nenhum sujeito foi selecionado";
                        return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Action NovoSujeito :: SujeitoController -> execute: " + ex.ToString());
                }
            }
            TempData["validacoes"] = "Não foi possivel realizar a agregação de sujeito";
            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.IdAnalise, projetoId = modelo.IdProjeto });
        }

    }
}