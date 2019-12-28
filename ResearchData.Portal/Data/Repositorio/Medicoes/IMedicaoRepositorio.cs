using ResearchData.Portal.Data.Repositorio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Medicoes;
using ResearchData.Portal.Models.ViewModels.Sujeitos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio.Medicoes
{
    public interface IMedicaoRepositorio
    {
        IQueryable<RetornoDeMedicoes> RetornarMedicoes(int idSujeito, int? IdExperimento);
        IList<MedicoesViewModel> MontarListaModeloMedicoes(int idSujeito);
        Task SalvarMedicoesComuns(int idSujeito, int idAnalise, MedicoesViewModel item);
        Task SalvarMedicoesExperimento(int idSujeito, int IdExperimento, MedicoesViewModel item);
        IList<ListaSujeitoSimplesViewModel> ObterSujeitosNoExperimento(int idAnalise, int idExperimento);
        IList<ListaSujeitoSimplesViewModel> ObterSujeitosDisponiveisExperimento(int idAnalise, int idExperimento);
        Task AddMedicaoExperimental(int idExperimento, int idSujeito);
        Task RemoveMedicaoExperimental(int idExperimento, int idSujeito);
        IList<MedicoesViewModel> MontarListaModeloMedicoesExp(int idSujeito, int IdExperimento);
        string[] BuscarResultadoParaCsv(int idAnalise, int idExperimento, int idSujeito);
    }
}
