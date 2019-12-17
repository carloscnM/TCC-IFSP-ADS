using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.Data.Repositorio.Analises;
using ResearchData.Portal.Data.Repositorio.Projetos;
using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.Models.ViewModels.Colaborador;
using Microsoft.Extensions.Localization;
using ResearchData.Portal.Data.Repositorio.Colaborador;

namespace ResearchData.Portal.Controllers.Colaborador
{
    [Authorize]
    public class ColaboradorController : Controller
    {
        private readonly IStringLocalizer<ColaboradorController> _localizador;
        private readonly IColaboradorRepositorio _colaRepo;
        private readonly UserManager<UsuarioAplicacao> _userManager;
        private readonly IAnaliseRepositorio _analiseRepo;
        private readonly IProjetoRepositorio _projetoRepo;

        public ColaboradorController(UserManager<UsuarioAplicacao> userManager,
                                        IAnaliseRepositorio analiseRepo,
                                            IProjetoRepositorio projetoRepo,
                                                IStringLocalizer<ColaboradorController> localizador,
                                                    IColaboradorRepositorio colaboradorRepo)
        {
            _userManager = userManager;
            _analiseRepo = analiseRepo;
            _projetoRepo = projetoRepo;
            _localizador = localizador;
            _colaRepo = colaboradorRepo;
        }


        public async Task<IActionResult> ListaDeAnalises()
        {
            var usuarioId = await ObterIdAsync();

            var projetosParaAgrupar = _projetoRepo.ObterInforBasicaPorColaborador(usuarioId);

            List<ListaDeAnaliseViewModel> modelo = new List<ListaDeAnaliseViewModel>();
            ListaDeAnaliseViewModel lda;


            foreach (var item in projetosParaAgrupar.Distinct())
            {
                lda = new ListaDeAnaliseViewModel();
                lda.IdProjeto = item;
                lda.TituloProjeto = await _projetoRepo.ObterTituloProjeto(item);
                lda.Analises = _analiseRepo.ObterAnaliseCompartilhadaPorProjeto(item, usuarioId);
                modelo.Add(lda);
            }

            if (TempData["msgCola1"] != null)
            {
                ViewBag.MsgColaDisplay = TempData["msgCola"];
            }
            return View(modelo);
        }


        public IActionResult RemoverColaborador(int IdAnalise, int IdProjeto, string ColaId)
        {
            DetalheColaboradorViewModel modelo = _colaRepo.BuscarDetalheColaborador(ColaId, IdAnalise);
            if (modelo != null)
            {
                modelo.IdProjeto = IdProjeto;
                return PartialView(modelo);
            }

            TempData["msg"] = 2;
            return RedirectToAction("AcessarProjeto", "Projetos", new { Id = IdProjeto });
        }

        [HttpPost]
        public async Task<IActionResult> RemoverColaborador(DetalheColaboradorViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool resultado = await _colaRepo.RemoverColaboradorAnalise(modelo.IdAnalise, modelo.Id);
                    if (resultado)
                    {
                        TempData["msg"] = 3;
                        return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.IdProjeto });
                    }
                    new InvalidOperationException();
                }
                catch (Exception)
                {

                }
            }
            TempData["msg"] = 2;
            return RedirectToAction("AcessarProjeto", "Projetos", new { Id = modelo.IdProjeto });
        }

        public async Task<IActionResult> SairDaAnalise(int idAnalise)
        {
            bool colaborador = _analiseRepo.VerificarColaborador(await ObterIdAsync().ConfigureAwait(false), idAnalise);
            if (colaborador)
            {
                SairAnaliseViewModel modelo = new SairAnaliseViewModel()
                {
                    IdAnalise = idAnalise,
                    TituloAnalise = await _analiseRepo.ObterTituloAnalise(idAnalise).ConfigureAwait(false),
                    TituloProjeto = await _projetoRepo.ObterTituloProjeto(_analiseRepo.ObterIdProjeto(idAnalise)).ConfigureAwait(false)
                };
                return PartialView(modelo);
            }
            return StatusCode(203);
        }

        [HttpPost]
        public async Task<IActionResult> SairDaAnalise(SairAnaliseViewModel modelo)
        {
            string msg = _localizador["Não foi possível Abandonar a análise"].ToString();
            if (ModelState.IsValid)
            {
                bool resultado = _colaRepo.SairDaAnalise(await ObterIdAsync(), modelo.IdAnalise);
                if (resultado)
                {
                    msg = _localizador["Análise Abandonada com sucesso"].ToString();
                    TempData["msgCola1"] = msg;
                    return RedirectToAction("ListaDeAnalises", "Colaborador");
                }
            }
            TempData["msgCola1"] = msg;
            return RedirectToAction("ListaDeAnalises", "Colaborador");
        }


        private async Task<string> ObterIdAsync()
        {
            var usuario = await _userManager.GetUserAsync(User);
            return Convert.ToString(usuario.Id);
        }
    }
}