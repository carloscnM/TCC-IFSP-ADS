using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.Data.Repositorio.Grupos;
using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Grupos;
using Microsoft.Extensions.Localization;
using ResearchData.Portal.Data.Repositorio.Analises;
using ResearchData.Portal.Data.Repositorio.Projetos;
using ResearchData.Portal.GerenciamentoUsuario;
using Microsoft.AspNetCore.Identity;

namespace ResearchData.Portal.Controllers.Grupos
{
    [Authorize]
    public class GrupoController : Controller
    {
        private readonly IStringLocalizer<GrupoController> _localizador;
        private readonly UserManager<UsuarioAplicacao> _userManager;
        private readonly IAnaliseRepositorio _repoAnalise;
        private readonly IProjetoRepositorio _repoProjeto;
        private IGrupoRepositorio _repoGrupo;

        public GrupoController(IGrupoRepositorio grupoRepositorio, 
                                    IStringLocalizer<GrupoController> localizador,
                                        IAnaliseRepositorio AnaliseRepo,
                                            IProjetoRepositorio projetoRepositorio,
                                                UserManager<UsuarioAplicacao> userManager)
        {
            this._repoGrupo = grupoRepositorio;
            this._localizador = localizador;
            this._repoAnalise = AnaliseRepo;
            this._repoProjeto = projetoRepositorio;
            this._userManager = userManager;
        }

        public IActionResult CadastrarGrupo(int IdAnalise, int IdProjeto)
        {
            GrupoViewModel modelo = new GrupoViewModel()
            {
                AnaliseId = IdAnalise,
                ProjetoId = IdProjeto
            };

            return PartialView(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarGrupo(GrupoViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                Grupo grupo = new Grupo()
                {
                    Nome = modelo.Nome,
                    Descricao = modelo.Descricao,
                    DataInclusao = DateTime.Now,
                    AnaliseOrigem = modelo.AnaliseId,
                    Ativo = true,
                    ProjetoId = modelo.ProjetoId
                };

                var resultado =  await _repoGrupo.CadastrarGrupo(grupo);
                if (resultado) {
                    TempData["validacoes"] = _localizador["Grupo cadastrado com sucesso!"].ToString();
                    return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId});
                }
            }
            TempData["validacoes"] = _localizador["Não foi possivel cadastrar o grupo!"].ToString();
            return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId});
        }

        public async Task<IActionResult> ListarGruposAcoes(int IdAnalise)
        {
            if (IdAnalise != 0)
            {
                IList<GrupoViewModel> modelo = _repoGrupo.BuscarListaGrupoAnalise(IdAnalise);
                var idProjeto = _repoAnalise.ObterIdProjeto(IdAnalise);
                bool responsavel = _repoProjeto.VerificarResposavel(await ObterIdAsync().ConfigureAwait(false), idProjeto);
                ViewBag.Resposavel = responsavel;
                return PartialView(modelo);
            }
            return StatusCode(203);
        }


        public async Task<IActionResult> EditarGrupo(int IdGrupo)
        {
            if (IdGrupo != 0)
            {
                var grupo = await _repoGrupo.BuscarGrupo(IdGrupo);

                GrupoViewModel modelo = new GrupoViewModel()
                {
                    Id = grupo.Id,
                    AnaliseId = grupo.AnaliseOrigem,
                    ProjetoId = grupo.ProjetoId,
                    Nome = grupo.Nome,
                    Descricao = grupo.Descricao
                };
                return PartialView(modelo);
            }
            return StatusCode(203);
        }

        [HttpPost]
        public async Task<IActionResult> EditarGrupo(GrupoViewModel modelo)
        {
            if (modelo.Id != 0)
            {
                var grupo = await _repoGrupo.BuscarGrupo(modelo.Id);

                grupo.Nome = modelo.Nome;
                grupo.Descricao = modelo.Descricao;

                var resultado = await _repoGrupo.AlterarGrupo(grupo);
                if (resultado) {
                    TempData["validacoes"] = _localizador["Grupo Alterado com sucesso!"].ToString();
                    return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId});
                }
                TempData["validacoes"] = _localizador["Não foi possivel realizar a alteração no grupo!"].ToString();
                return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId});
            }
            return StatusCode(203);
        }

        public async Task<IActionResult> DesativarGrupo(int IdGrupo)
        {
            if (IdGrupo != 0)
            {
                var grupo = await _repoGrupo.BuscarGrupo(IdGrupo);

                GrupoViewModel modelo = new GrupoViewModel()
                {
                    Id = grupo.Id,
                    AnaliseId = grupo.AnaliseOrigem,
                    ProjetoId = grupo.ProjetoId,
                    Nome = grupo.Nome,
                    Descricao = grupo.Descricao
                };
                return PartialView(modelo);
            }
            return StatusCode(203);
        }

        [HttpPost]
        public async Task<IActionResult> DesativarGrupo(GrupoViewModel modelo)
        {
            if (modelo.Id != 0)
            {
                var grupo = await _repoGrupo.BuscarGrupo(modelo.Id);

                grupo.Ativo = false;

                var resultado = await _repoGrupo.AlterarGrupo(grupo);
                if (resultado) {
                    TempData["validacoes"] = _localizador["Grupo desativado com sucesso!"].ToString();
                    return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId});
                }
                TempData["validacoes"] = _localizador["Não foi possível desativar o grupo!"].ToString();
                return RedirectToAction("AcessarAnalise", "Analise", new { analiseId = modelo.AnaliseId, projetoId = modelo.ProjetoId});
            }
            return StatusCode(203);
        }

        private async Task<string> ObterIdAsync()
        {
            var usuario = await _userManager.GetUserAsync(User);
            return Convert.ToString(usuario.Id);
        }

    }
}