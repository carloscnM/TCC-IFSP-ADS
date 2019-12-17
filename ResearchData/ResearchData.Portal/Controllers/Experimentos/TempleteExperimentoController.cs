using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ResearchData.Portal.Data.Repositorio.Caracteristicas;
using ResearchData.Portal.Data.Repositorio.Experimentos;
using ResearchData.Portal.Models.Negocio.Experimentos;
using ResearchData.Portal.Models.ViewModels.Experimentos;

namespace ResearchData.Portal.Controllers.Experimentos
{
    [Authorize]
    public class TempleteExperimentoController : Controller
    {
        private readonly IStringLocalizer<TempleteExperimentoController> _localizador;
        private ICaracteristicaRepositorio _caraRepo;
        private IExperimentoRepositorio _repoExperi;

        public TempleteExperimentoController(IStringLocalizer<TempleteExperimentoController> localizador, ICaracteristicaRepositorio caracRepo, IExperimentoRepositorio experimentoRepositorio)
        {
            this._localizador = localizador;
            _caraRepo = caracRepo;
            _repoExperi = experimentoRepositorio;
        }

        public IActionResult CriarExperimentoTemplete()
        {
            CriarTempleteViewModel modelo = new CriarTempleteViewModel()
            {
                Nome = "",
                Descricao = "",
                Caracteristicas = _caraRepo.ListaTodasCaracteristicaEspecificas()
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> CriarExperimentoTemplete(CriarTempleteViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Experimento experimento = new Experimento()
                    {
                        Nome = modelo.Nome,
                        Descricao = modelo.Descricao
                    };




                    if (modelo.Caracteristicas != null)
                    {
                        var selecionadas = modelo.Caracteristicas.Where(car => car.IsChecked == true);
                        if (selecionadas.Count() > 0)
                        {
                            var idExperimento = await _repoExperi.AddExperimento(experimento);
                            TempData["msg"] = "1";
                            foreach (var item in selecionadas)
                            {
                                await _repoExperi.AddGrupoDeDadosParaExp(idExperimento, item.CaracteristicaId).ConfigureAwait(false);
                            }

                            return RedirectToAction("ConsultarExperimento", "TempleteExperimento", new { IdExperimento = idExperimento });
                        }
                        ViewBag.SemCaracteristica = _localizador["Necessário ao menos uma caracteristica para o experimento!"].ToString();
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
            ViewBag.CadatroSucesso = false;
            return View(modelo);
        }

        public IActionResult SelConExperimento()
        {
            IList<ExpSimplesViewModel> modelo = _repoExperi.BuscarExperimentos();
            return PartialView(modelo);
        }

        public async Task<IActionResult> ConsultarExperimento(int IdExperimento)
        {

            ConsultaExpViewModel modelo = new ConsultaExpViewModel()
            {
                Nome = await _repoExperi.ObterNomeExperimento(IdExperimento),
                Descricao = _repoExperi.ObterDescricaoExperimento(IdExperimento),
                Lista = _repoExperi.BuscarGrupoDeDados(IdExperimento)
            };

            if (TempData["msg"] != null)
            {
                ViewData["CadastroNovo"] = _localizador["Modelo de experimento cadastrado com sucesso!"].ToString();
            }

            return View(modelo);
        }
    }
}