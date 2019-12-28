using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Grupos;
using ResearchData.Portal.Models.ViewModels.Relatorios;

namespace ResearchData.Portal.Data.Repositorio.Grupos
{
    public interface IGrupoRepositorio
    {
        Task<bool> CadastrarGrupo(Grupo grupo);
        Task<IEnumerable<Grupo>> BuscarGrupoPorProjeto(int idProjeto);
        Task<Grupo> BuscarGrupo(int idGrupo);
        Task<bool> AlterarGrupo(Grupo grupo);

        IList<GrupoViewModel> BuscarListaGrupoAnalise(int idAnalise);
        IList<SujeitosPorGrupoViewModel> ObterQtdSujeitosPorGrupo(int idAnalise);
    }
}
