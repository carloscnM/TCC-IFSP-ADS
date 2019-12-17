using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ResearchData.Portal.Data.Repositorio.Comentarios;
using ResearchData.Portal.GerenciamentoUsuario.PerfisDeUsuario;
using ResearchData.Portal.Models.ViewModels.Comentarios;

namespace ResearchData.Portal.Controllers.Comentarios
{
    [Authorize(Roles = PerfisPadroes.USUARIO)]
    public class ComentariosController : Controller
    {
        private readonly IStringLocalizer<AccountController> _localizador;
        private IComentarioRepositorio _comenRepositorio;

        public ComentariosController(IStringLocalizer<AccountController> localizador,
                                        IComentarioRepositorio comentarioRepositorio)
        {
            this._localizador = localizador;
            this._comenRepositorio = comentarioRepositorio;
        }

        public IActionResult Comentarios(int IdAnalise)
        {
            ComentarioAnaliseViewModel modelo = new ComentarioAnaliseViewModel()
            {
                AnaliseId = IdAnalise,
                Comentarios = _comenRepositorio.ListaComenatarioAnalise(IdAnalise)
            };

            return PartialView(modelo);
        }

        [HttpPost]
        public async Task CadastrarComentario(NovoComentarioViewModel modelo)
        {
            await _comenRepositorio.AdicionarComentario(modelo);
        }



    }
}