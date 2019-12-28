using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchData.Portal.Models.Negocio.Experimentos;
using ResearchData.Portal.Models.ViewModels.Caracteristicas;
using ResearchData.Portal.Models.ViewModels.Experimentos;

namespace ResearchData.Portal.Data.Repositorio.Experimentos
{
    public interface IExperimentoRepositorio
    {
        IList<ExpSimplesViewModel> BuscarExperimentos();
        Task<string> ObterNomeExperimento(int idExperimento);

        Task<int> AddExperimento(Experimento experimento);
        Task AddGrupoDeDadosParaExp(int idExperimento, int caracteristicaId);
        string ObterDescricaoExperimento(int idExperimento);
        IList<ListaDeCaracteristicasViewModel> BuscarGrupoDeDados(int idExperimento);
        IList<string> BuscarNomeCaracteriscaExp(int idExperimento);
    }
}
