using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.Data.Repositorio.Analises;
using ResearchData.Portal.Data.Repositorio.Experimentos;
using ResearchData.Portal.Models.ViewModels.Experimentos;
using Microsoft.Extensions.Localization;

namespace ResearchData.Portal.Controllers.Experimentos
{
    [Authorize]
    public class ExperimentoController : Controller
    {
        private readonly IStringLocalizer<ExperimentoController> _localizador;
        private IAnaliseRepositorio _repoAnalise;
        private IExperimentoRepositorio _repoExperi;

        public ExperimentoController(IAnaliseRepositorio AnaliseRepo, IExperimentoRepositorio experimentoRepositorio, IStringLocalizer<ExperimentoController> localizador)
        {
            this._repoAnalise = AnaliseRepo;
            this._repoExperi = experimentoRepositorio;
            this._localizador = localizador;
        }

        public async Task<IActionResult> SelExperimento(int IdAnalise, int IdProjeto)
        {
            SelExperimentoViewModel modelo = new SelExperimentoViewModel()
            {
                IdAnalise = IdAnalise,
                IdProjeto = IdProjeto,
                TituloAnalise = await _repoAnalise.ObterTituloAnalise(IdAnalise),
                ListaExp = _repoExperi.BuscarExperimentos()
            };
            return PartialView(modelo);
        }

    }
}