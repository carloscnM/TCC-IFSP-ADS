using ResearchData.Portal.Data.Repositorio.Projetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.Models.ViewModels.Grupos;
using ResearchData.Portal.Models.ViewModels.Relatorios;
using ResearchData.Portal.Models.Negocio.Analises;

namespace ResearchData.Portal.Data.Repositorio.Grupos
{
    public class GrupoRepositorio : BaseRepositorio<Grupo>, IGrupoRepositorio
    {
        public GrupoRepositorio(RDContextoDaAplicacao contexto) : base(contexto)
        {

        }

        public async Task<bool> AlterarGrupo(Grupo grupo)
        {
            try
            {
                contexto.Set<Grupo>().Update(grupo);
                await contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception) { }
            return false;
        }

        public async Task<Grupo> BuscarGrupo(int idGrupo)
        {
            return await contexto.Set<Grupo>().FindAsync(idGrupo);
        }

        public async Task<IEnumerable<Grupo>> BuscarGrupoPorProjeto(int idProjeto)
        {
            var grupos = contexto.Set<Grupo>().Where(gru => gru.ProjetoId == idProjeto & gru.Ativo == true).OrderBy(gru => gru.Nome).ToAsyncEnumerable();
            return grupos.ToEnumerable();
        }

        public IList<GrupoViewModel> BuscarListaGrupoAnalise(int idAnalise)
        {
            var projetoId = contexto.Set<Analise>().Find(idAnalise).ProjetoId;
            return contexto.Set<Grupo>()
                    .Where(gru => gru.ProjetoId == projetoId & gru.Ativo == true)
                        .Select(gru => new GrupoViewModel()
                            { Id = gru.Id, AnaliseId = gru.AnaliseOrigem, Nome = gru.Nome, ProjetoId = gru.ProjetoId })
                                .ToList();
        }

        public async Task<bool> CadastrarGrupo(Grupo grupo)
        {
            try
            {
                await contexto.Set<Grupo>().AddAsync(grupo);
                await contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception) { }
            return false;
        }

        public IList<SujeitosPorGrupoViewModel> ObterQtdSujeitosPorGrupo(int idAnalise)
        {
            IList<SujeitosPorGrupoViewModel> modelo = new List<SujeitosPorGrupoViewModel>();
            if(idAnalise != 0)
            {
                var grupos = contexto.Set<Grupo>().Where(g => g.AnaliseOrigem == idAnalise).Select(g => new { IdGrupo = g.Id, GrupoAtivo = g.Ativo, GrupoNome = g.Nome }).ToList();

                SujeitosPorGrupoViewModel itemModelo;

                foreach (var item in grupos)
                {
                    var quantidadeSujeito = from suj in contexto.Set<SujeitoExperimental>()
                                            where suj.AnaliseId == idAnalise && suj.GrupoId == item.IdGrupo
                                            select suj.Id;

                    itemModelo = new SujeitosPorGrupoViewModel()
                    {
                        GrupoNome = item.GrupoNome,
                        QuantidadeSujeito = quantidadeSujeito.Count()
                    };

                    if(quantidadeSujeito.Count() == 0 & item.GrupoAtivo == false)
                        break;

                    modelo.Add(itemModelo);
                }
                return modelo;
            }

            return modelo;
        }

    }
}
