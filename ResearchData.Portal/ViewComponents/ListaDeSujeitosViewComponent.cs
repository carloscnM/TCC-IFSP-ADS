using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.Data.Repositorio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Sujeitos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.ViewComponents
{
    public class ListaDeSujeitosViewComponent : ViewComponent
    {
        private ISujeitoRepositorio _repoSujeito;

        public ListaDeSujeitosViewComponent(ISujeitoRepositorio projetoRepositorio)
        {
            _repoSujeito = projetoRepositorio;
        }

        public async Task<IViewComponentResult> InvokeAsync(int IdAnalise, int idProjeto)
        {
            SujeitosComponeteViewModel modelo = new SujeitosComponeteViewModel()
            {
                IdAnalise = IdAnalise,
                IdProjeto = idProjeto,
                AnaliseAtiva = _repoSujeito.VerificarAnaliseAtiva(IdAnalise),
                Sujeitos = await GetItemsAsync(IdAnalise, idProjeto)
            };
            return View(modelo);
        }

        private Task<IEnumerable<ListaSujeitosViewModel>> GetItemsAsync(int IdAnalise, int Idprojeto)
        {
            return _repoSujeito.BuscarSujeitosAnalise(IdAnalise, Idprojeto);
        }
    }
}
