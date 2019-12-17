using ResearchData.Portal.Models.Projetos.Negocio;
using ResearchData.Portal.Models.ViewModels.Colaborador;
using ResearchData.Portal.Models.ViewModels.Projetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio.Projetos
{
    public interface IProjetoRepositorio
    {
        DadosSimplesProjetoViewModel ObterDetalheProjetoSimplificado(int IdProjeto);

        IList<DadosSimplesProjetoViewModel> ObterProjetosRecente(string usuarioId, int qtd);
        int ObterQuantidadeProjetoAndamento(string usuarioId, bool aberto);

        int ObterQuantidadeColaboracoes(string usuarioId);
        //Obter Detalhes
        Task<IEnumerable<Projeto>> ProjetoPorUsuario(string usuarioID);


        Task<IEnumerable<Projeto>> ProjetoPorUsuarioUserName(string userName);


        Task AddProjeto(Projeto projeto);


        Task<Projeto> BuscarProjetoDoUsuario(int idProjeto, string IdUsuario);


        Task<int> BuscarIdProjetoPorAnalise(int idAnalise);


        Task<string> ObterTituloProjeto(int idProjeto);


        Task<Projeto> BuscarProjetoPorId(int Id);


        Task<DetalhesProjetoViewModel> DetalhesProjeto(int Id);


        ResponsavelDoProjetoViewModel ObterResponsavel(int idProjeto);


        
        IEnumerable<int> ObterInforBasicaPorColaborador(string usuarioId);




        //Modificaçoes
        Task<bool> AlterarProjeto(Projeto projeto);
        Task ExcluirProjeto(Projeto projeto);



        //Verificações
        bool VerificarResposavel(string UserId, int idProjeto);
    }
}
