using ResearchData.Portal.Models.Negocio.Analises;
using ResearchData.Portal.Models.ViewModels.Analises;
using ResearchData.Portal.Models.ViewModels.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio.Analises
{
    public interface IAnaliseRepositorio
    {
        Task AddAnalise(Analise analise);

        IList<Analise> AnalisePorProjeto(int projetoId);

        Task<Analise> BuscarAnalise(int analiseId);

        int ObterIdProjeto(int analiseId);

        IEnumerable<ListaColaboradorViewModel> BuscarColaboradoresDaAnalise(int analiseId);

        IEnumerable<Analise> ObterAnaliseCompartilhadaPorProjeto(int projetoId, string usuarioId);

        bool AdicionarColaborador(int idAnalise, string email, string usuarioId, int projetoId, int acesso);

        Task<string> ObterTituloAnalise(int idAnalise);

        bool ExisteSujeito(int analiseId);

        Task EncerraAnalise(int analiseId);

        bool VerificarColaborador(string UserId, int idAnalise);

        Task<DetalheAnaliseViewModel> DetalhesAnalise(int idAnalise);

        MetricasAnaliseViewModel DetalheMetricaAnalise(int IdAnalise);

        IList<SujeitosPorExperimentoViewModel> RelacaoSujeitosPorExperimento(int IdAnalise);

        IList<int?> ListarExperimentoDaAnalise(int IdAnalise);

        IList<int> TodosSujeitoExperimentoAnalise(int IdAnalise, int IdExperimento);
        bool AlterarAnalise(Analise analise);
        Task DevativarAnalisesDoProjeto(int id);
        int ConsultarAcesso(string colaboradorId);
    }
}
