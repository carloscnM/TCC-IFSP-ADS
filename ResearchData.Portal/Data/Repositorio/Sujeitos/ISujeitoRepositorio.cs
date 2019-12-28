using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Experimentos;
using ResearchData.Portal.Models.ViewModels.Sujeitos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio.Sujeitos
{
    public interface ISujeitoRepositorio
    {
        Task<IEnumerable<ListaSujeitosViewModel>> BuscarSujeitosAnalise(int IdAnalise, int IdProjeto);
        string BuscarDescricaoPorId(int idSujeito);
        IEnumerable<MedicaoComunsViewModel> BuscarMedicaoComunDoSujeito(int idSujeito);
        IEnumerable<ListaSujeitoSimplesViewModel> ListarSujeitosAnaliseSimples(int idAnalise);

        Task<bool> CadastrarSujeitoAnalise(SujeitoExperimental sujeito);
        IList<ExpSimplesViewModel> BuscarExpDoSujeito(int idSujeito);
        IEnumerable<MedicaoComunsViewModel> BuscarMedicaoComunDoSujExperimento(int IdSujeito, int IdExperimento);
        bool VerificarAnaliseAtiva(int idAnalise);
        NovoSujeitoViewModel ObterDetalheSujeito(int idSujeito);
        Task<bool> EditarSujeito(NovoSujeitoViewModel modelo);
        Task<bool> RemoverSujeito(NovoSujeitoViewModel modelo);
        IList<ListaSujeitoSimplesViewModel> BuscarSujeitoExistente(int idAnalise, int idProjeto);
    }
}
