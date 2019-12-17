using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.Data.Repositorio.Projetos;
using ResearchData.Portal.Models.Projetos.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.ViewComponents
{
    public class ListaDeProjetosViewComponent : ViewComponent
    {
        private IProjetoRepositorio _repoProjeto;

        public ListaDeProjetosViewComponent(IProjetoRepositorio projetoRepositorio)
        {
            _repoProjeto = projetoRepositorio;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userName)
        {
            var projetos = await GetItemsAsync(userName);
            return View(projetos);
        }

        private Task<IEnumerable<Projeto>> GetItemsAsync(string userName)
        {
            return _repoProjeto.ProjetoPorUsuarioUserName(userName);
        }

    }
}
