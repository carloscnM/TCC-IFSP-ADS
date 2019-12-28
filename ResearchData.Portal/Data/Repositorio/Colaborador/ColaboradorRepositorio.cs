using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.Models.Negocio.Analises;
using ResearchData.Portal.Models.ViewModels.Colaborador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio.Colaborador
{
    public class ColaboradorRepositorio : BaseRepositorio<ColaboradorAnalise>, IColaboradorRepositorio
    {
        public ColaboradorRepositorio(RDContextoDaAplicacao contexto): base(contexto)
        {

        }

        public DetalheColaboradorViewModel BuscarDetalheColaborador(string colaId, int IdAnalise)
        {
            var detalhe = from cola in contexto.Set<ColaboradorAnalise>()
                          join use in contexto.Users on cola.UsuarioAplicacaoId equals use.Id
                          where cola.UsuarioAplicacaoId == colaId && cola.AnaliseId == IdAnalise
                          select new DetalheColaboradorViewModel()
                          {
                              Id = cola.UsuarioAplicacaoId,
                              Nome = use.Nome,
                              Email = use.Email,
                              IdAnalise = cola.AnaliseId,
                              DtInclusao = cola.DataInclusao
                          };

            return detalhe.FirstOrDefault();
        }

        public string ObterNomeColaborador(string email)
        {
            var nome = contexto.Users.Where(u => u.Email == email).Select(u => u.Nome).FirstOrDefault();
            return nome.Substring(0, nome.IndexOf(" "));
        }

        public async Task<bool> RemoverColaboradorAnalise(int idAnalise, string ColaId)
        {
            try
            {
                ColaboradorAnalise modelo = new ColaboradorAnalise()
                {
                    AnaliseId = idAnalise,
                    UsuarioAplicacaoId = ColaId
                };
                contexto.Set<ColaboradorAnalise>().Remove(modelo);
                await contexto.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task RemoverColaboradores(int IdAnalise)
        {
            contexto.Set<ColaboradorAnalise>()
                        .RemoveRange(contexto.Set<ColaboradorAnalise>()
                            .Where(co => co.AnaliseId == IdAnalise)
                                .ToList());

            await contexto.SaveChangesAsync().ConfigureAwait(false);
        }

        public bool SairDaAnalise(string userId, int idAnalise)
        {
            try
            {
                var colaboracao = contexto.Set<ColaboradorAnalise>().Where(cola => cola.AnaliseId == idAnalise & cola.UsuarioAplicacaoId == userId).FirstOrDefault();
                contexto.Set<ColaboradorAnalise>().Remove(colaboracao);
                contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
