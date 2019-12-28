using Microsoft.EntityFrameworkCore.Internal;
using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.Models.Negocio.Analises;
using ResearchData.Portal.Models.Projetos.Negocio;
using ResearchData.Portal.Models.ViewModels.Colaborador;
using ResearchData.Portal.Models.ViewModels.Projetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio.Projetos
{
    public class ProjetoRepositorio : BaseRepositorio<Projeto>, IProjetoRepositorio
    {

        public ProjetoRepositorio(RDContextoDaAplicacao contexto) : base(contexto)
        {

        }


        public async Task AddProjeto(Projeto projeto)
        {
            await contexto.Set<Projeto>().AddAsync(projeto).ConfigureAwait(false);
            await contexto.SaveChangesAsync().ConfigureAwait(false);
        }



        public async Task<IEnumerable<Projeto>> ProjetoPorUsuario(string usuarioID)
        {
            var projetos = contexto.Set<Projeto>().ToList().Where(p => p.UsuarioAplicacaoId == usuarioID & p.Ativo == true);
            return projetos;
        }


        public async Task<Projeto> BuscarProjetoDoUsuario(int idProjeto, string IdUsuario)
        {
            var projeto = await contexto.Set<Projeto>().FindAsync(idProjeto).ConfigureAwait(false);

            if (projeto.UsuarioAplicacaoId == IdUsuario)
            {
                return projeto;
            }
            return null;
        }

        public async Task<string> ObterTituloProjeto(int idProjeto)
        {
            var projeto = await contexto.Set<Projeto>().FindAsync(idProjeto).ConfigureAwait(false);

            return projeto.Titulo;
        }

        public ResponsavelDoProjetoViewModel ObterResponsavel(int idProjeto)
        {
            var responsavel = from usu in contexto.Users
                              join pro in contexto.Set<Projeto>() on usu.Id equals pro.UsuarioAplicacaoId
                              where pro.Id == idProjeto
                              select new ResponsavelDoProjetoViewModel()
                              {
                                  Email = usu.Email,
                                  Nome = usu.Nome
                              };

            return responsavel.FirstOrDefault();
        }

        public IEnumerable<int> ObterInforBasicaPorColaborador(string usuarioId)
        {
            var listaProjeto = from pro in contexto.Set<Projeto>()
                               join ana in contexto.Set<Analise>() on pro.Id equals ana.ProjetoId
                               join cola in contexto.Set<ColaboradorAnalise>() on ana.Id equals cola.AnaliseId
                               where cola.UsuarioAplicacaoId == usuarioId & pro.Ativo == true & ana.Ativa == true
                               select pro.Id;
                                                          
            return listaProjeto.Distinct();
        }

        public async Task<IEnumerable<Projeto>> ProjetoPorUsuarioUserName(string userName)
        {
            var usuarioID = from usu in contexto.Set<UsuarioAplicacao>()
                            where usu.UserName == userName 
                            select usu.Id;

            string Id = usuarioID.First();

            var projetos = contexto.Set<Projeto>().ToList().Where(p => p.UsuarioAplicacaoId == Id & p.Ativo == true).OrderByDescending(p => p.DataInicial);

            return projetos;
        }

        public async Task<Projeto> BuscarProjetoPorId(int Id)
        {
            return await contexto.Set<Projeto>().FindAsync(Id);
        }

        public async Task<bool> AlterarProjeto(Projeto projetoModelo)
        {
            try
            {
                var projeto = await contexto.Set<Projeto>().FindAsync(projetoModelo.Id).ConfigureAwait(false);
                projeto.Titulo = projetoModelo.Titulo;
                projeto.Descricao = projetoModelo.Descricao;
                contexto.Set<Projeto>().Update(projeto);
                await contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

            }
            return false; 
        }

        public async Task ExcluirProjeto(Projeto projeto)
        {
            contexto.Set<Projeto>().Update(projeto);
            await contexto.SaveChangesAsync().ConfigureAwait(true);
        }

        public async Task<int> BuscarIdProjetoPorAnalise(int idAnalise)
        {
            var analise = await contexto.Set<Analise>().FindAsync(idAnalise).ConfigureAwait(true);
            return analise.ProjetoId;
        }

        public bool VerificarResposavel(string UserId, int idProjeto)
        {
            var resultado = contexto.Set<Projeto>()
                                    .ToList().Where(p => p.Id == idProjeto && p.UsuarioAplicacaoId == UserId)
                                             .Count();

            if (resultado > 0)
                return true;

            return false;
        }

        public async Task<DetalhesProjetoViewModel> DetalhesProjeto(int Id)
        {
            var Detalhes = from pro in contexto.Set<Projeto>()
                           where pro.Id == Id & pro.Ativo == true
                           select new DetalhesProjetoViewModel()
                           {
                               Id = pro.Id,
                               Titulo = pro.Titulo,
                               Descricao = pro.Descricao,
                               DtInicio = pro.DataInicial,
                               DtFim = pro.DataFinal
                           };

            Detalhes.FirstOrDefault().QuatidadeAnalise = contexto.Set<Analise>().Where(ana => ana.ProjetoId == Id & ana.Ativa == true).Count();
            return Detalhes.FirstOrDefault();
        }

        public IList<DadosSimplesProjetoViewModel> ObterProjetosRecente(string usuarioId, int qtd)
        {
            var projetos = contexto.Set<Projeto>()
                                        .Where(pro => pro.UsuarioAplicacaoId == usuarioId & pro.Ativo == true & pro.DataFinal == null)
                                            .OrderByDescending(pro => pro.DataInicial)
                                                .Select(pro => new DadosSimplesProjetoViewModel { IdProjeto = pro.Id, TituloProjeto = pro.Titulo, DataCadastro = pro.DataInicial})
                                                    .Take(qtd);
            return projetos.ToList();
        }

        public int ObterQuantidadeProjetoAndamento(string usuarioId, bool aberto)
        {
            if (aberto)
                return  contexto.Set<Projeto>().Where(p => p.UsuarioAplicacaoId == usuarioId & p.DataFinal == null & p.Ativo == true).Count();
            return contexto.Set<Projeto>().Where(p => p.UsuarioAplicacaoId == usuarioId & p.DataFinal != null & p.Ativo == true).Count();
        }

        public int ObterQuantidadeColaboracoes(string usuarioId)
        {
            var quatidadeCola = from cola in contexto.Set<ColaboradorAnalise>()
                            join ana in contexto.Set<Analise>() on cola.AnaliseId equals ana.Id
                            where ana.Ativa == true & cola.UsuarioAplicacaoId == usuarioId
                            select cola.AnaliseId;

            return quatidadeCola.Count();
        }

        public DadosSimplesProjetoViewModel ObterDetalheProjetoSimplificado(int IdProjeto)
        {
            return contexto.Set<Projeto>()
                                .Where(p => p.Id == IdProjeto)
                                    .Select(p => new DadosSimplesProjetoViewModel { IdProjeto = p.Id, TituloProjeto = p.Titulo, DataCadastro = p.DataInicial })
                                        .FirstOrDefault();
        }
    }
}
