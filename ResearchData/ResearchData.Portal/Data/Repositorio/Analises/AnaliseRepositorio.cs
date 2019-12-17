using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.Models.Negocio.Analises;
using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.Projetos.Negocio;
using ResearchData.Portal.Models.ViewModels.Analises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ResearchData.Portal.Models.ViewModels.Colaborador;
using ResearchData.Portal.Models.ViewModels.Relatorios;
using ResearchData.Portal.Models.Negocio.Experimentos;

namespace ResearchData.Portal.Data.Repositorio.Analises
{
    public class AnaliseRepositorio : BaseRepositorio<Analise>, IAnaliseRepositorio
    {
        private readonly ILogger _logger;

        public AnaliseRepositorio(RDContextoDaAplicacao contexto, ILogger<AnaliseRepositorio> logger) : base(contexto)
        {
            this._logger = logger;
        }

        public async Task AddAnalise(Analise analise)
        {
            try
            {
                analise.Ativa = true;
                await contexto.Set<Analise>().AddAsync(analise);
                await contexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Action AddAnalise :: AnaliseRepositorio -> execute: " + ex.ToString());
                throw;
            }
        }

        public IList<Analise> AnalisePorProjeto(int projetoId)
        {
            var analises = contexto.Set<Analise>().ToList().Where(ana => ana.ProjetoId == projetoId & ana.Ativa == true).ToList();
            return analises;
        }

        public async Task<Analise> BuscarAnalise(int analiseId)
        {
            var analise = await contexto.Set<Analise>().FindAsync(analiseId);
            return analise;
        }

        public IEnumerable<ListaColaboradorViewModel> BuscarColaboradoresDaAnalise(int analiseId)
        {
            IEnumerable<ListaColaboradorViewModel> colaboradores = from usu in contexto.Users
                                                                   join ca in contexto.Set<ColaboradorAnalise>() on usu.Id equals ca.UsuarioAplicacaoId
                                                                   where ca.AnaliseId == analiseId
                                                                   select new ListaColaboradorViewModel()
                                                                   {
                                                                       UsuarioId = usu.Id,
                                                                       Email = usu.Email,
                                                                       DataCadastro = ca.DataInclusao,
                                                                       Acesso = ca.Acesso
                                                                   };

            return colaboradores;
        }

        public IEnumerable<Analise> ObterAnaliseCompartilhadaPorProjeto(int projetoId, string usuarioId)
        {
            var analises = from ana in contexto.Set<Analise>()
                           join ca in contexto.Set<ColaboradorAnalise>() on ana.Id equals ca.AnaliseId
                           where ana.ProjetoId == projetoId & ca.UsuarioAplicacaoId == usuarioId & ana.Ativa == true
                           select new Analise()
                           {
                               Id = ana.Id,
                               Nome = ana.Nome,
                               Descricao = ana.Descricao
                           };
            return analises;

        }


        public bool AdicionarColaborador(int idAnalise, string email, string usuarioId, int projetoId, int acesso)
        {
            var usuarioAut = from pro in contexto.Set<Projeto>()
                             where pro.Id == projetoId & pro.UsuarioAplicacaoId == usuarioId
                             select pro;

            if (usuarioAut.Count() == 1)
            {
                try
                {
                    //throw  new FormatException("erro Obter-Analise-Compartilhada-Por-Projeto");
                    var idColaborador = from usu in contexto.Users
                                        where usu.Email == email
                                        select usu.Id;

                    ColaboradorAnalise colaborador = new ColaboradorAnalise()
                    {
                        UsuarioAplicacaoId = idColaborador.First(),
                        AnaliseId = idAnalise,
                        Acesso = acesso
                    };

                    contexto.Set<ColaboradorAnalise>().Add(colaborador);
                    contexto.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Action ObterAnaliseCompartilhadaPorProjeto :: AnaliseRepositorio -> execute: " + ex.Message);
                }
            }
            return false;
        }

        public async Task<string> ObterTituloAnalise(int idAnalise)
        {
            var titulo = await contexto.Set<Analise>().FindAsync(idAnalise);
            return titulo.Nome;
        }

        public bool ExisteSujeito(int analiseId)
        {
            var sujeitos = from su in contexto.Set<SujeitoExperimental>()
                           where su.AnaliseId == analiseId
                           select su.Id;

            if (sujeitos.Count() > 0)
                return true;
            return false;
        }

        public async Task EncerraAnalise(int AnaliseId)
        {
            try
            {
                var analise = await contexto.Set<Analise>().FindAsync(AnaliseId);
                analise.DataFim = DateTime.Now;
                contexto.Set<Analise>().Update(analise);
                await contexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Metodo :: EncerrarAnalise :: Analise Repositorio -> execute" + ex.Message);
                throw;
            }
        }

        public bool VerificarColaborador(string UserId, int idAnalise)
        {
            var HeColaborador = from cola in contexto.Set<ColaboradorAnalise>()
                                where cola.UsuarioAplicacaoId == UserId & cola.AnaliseId == idAnalise
                                select cola.AnaliseId;

            if (HeColaborador.Any())
                return true;
            return false;
        }

        public async Task<DetalheAnaliseViewModel> DetalhesAnalise(int idAnalise)
        {
            DetalheAnaliseViewModel modelo = new DetalheAnaliseViewModel();
            try
            {
                var analise = await contexto.Set<Analise>().FindAsync(idAnalise);
                var colaboradores = from cola in contexto.Set<ColaboradorAnalise>()
                                    join use in contexto.Users on cola.UsuarioAplicacaoId equals use.Id
                                    where cola.AnaliseId == idAnalise
                                    select new DetalheColaboradorViewModel()
                                    {
                                        Nome = use.Nome,
                                        DtInclusao = cola.DataInclusao
                                    };



                modelo.Titulo = analise.Nome;
                modelo.Descricao = analise.Descricao;
                modelo.DtInicio = analise.DateInicio;
                modelo.DtFim = analise.DataFim;
                modelo.Colaboradores = colaboradores.ToList();

                //populando contadores
                
                if (analise.DataFim != null)
                    modelo.QuatidadeDias = Convert.ToInt32(Convert.ToDateTime(analise.DataFim).Subtract(analise.DateInicio).TotalDays);
                modelo.QuatidadeDias = Convert.ToInt32(DateTime.Now.Subtract(analise.DateInicio).TotalDays);

                
                

                modelo.QuatidadeSujeito = contexto.Set<SujeitoExperimental>().Where(su => su.AnaliseId == idAnalise).Count();

                var contarGrupo = from su in contexto.Set<SujeitoExperimental>()
                                  join gru in contexto.Set<Grupo>() on su.GrupoId equals gru.Id
                                  where su.AnaliseId == idAnalise
                                  select gru.Id;
                modelo.QuantidadeGrupo = contarGrupo.ToList().Distinct().Count();

                var contaExperimento = from su in contexto.Set<SujeitoExperimental>()
                                       join med in contexto.Set<Medicao>() on su.Id equals med.SujeitoExperimentalId
                                       where su.AnaliseId == idAnalise
                                       select med.SujeitoExperimentalId;

                modelo.QuatidadeExp = contaExperimento.ToList().Distinct().Count();




                return modelo;
            }
            catch (Exception)
            {

            }
            return modelo;

        }

        public MetricasAnaliseViewModel DetalheMetricaAnalise(int IdAnalise)
        {
            MetricasAnaliseViewModel modelo = new MetricasAnaliseViewModel()
            {
                TotalMedicoes = 0,
                MedicoesCaptadas = 0,
                MediaMedicoesPorSujeito = 0,
                MediaMedicoesPorExperimento = 0
            };

            try
            {
                var TotalMedicoes = from med in contexto.Set<Medicao>()
                                    join suj in contexto.Set<SujeitoExperimental>()
                                        on med.SujeitoExperimentalId equals suj.Id
                                    where suj.AnaliseId == IdAnalise
                                    select new { med.Id, med.DataCaptacao, med.SujeitoExperimentalId, med.ExperimentoId };

                modelo.TotalMedicoes = TotalMedicoes.Count();
                if (modelo.TotalMedicoes > 0)
                {
                    modelo.MedicoesCaptadas = TotalMedicoes.Where(med => med.DataCaptacao != null).Count();

                    var qtdSujeito = TotalMedicoes.Select(med => med.SujeitoExperimentalId).Distinct().Count();
                    modelo.MediaMedicoesPorSujeito = modelo.TotalMedicoes / qtdSujeito;

                    var qtdExperimento = TotalMedicoes.Where(med => med.ExperimentoId != null).Select(med => med.ExperimentoId).Distinct().Count();
                    modelo.MediaMedicoesPorExperimento = modelo.TotalMedicoes / qtdExperimento;
                }


                return modelo;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro durante levantamento de metrica Analise {IdAnalise}. execute: {ex.Message}");
            }

            return modelo;
        }

        public IList<SujeitosPorExperimentoViewModel> RelacaoSujeitosPorExperimento(int IdAnalise)
        {
            IList<SujeitosPorExperimentoViewModel> modelo = new List<SujeitosPorExperimentoViewModel>();
            try
            {
                if (IdAnalise == 0)
                    new InvalidOperationException("Análise Invalida");



                SujeitosPorExperimentoViewModel itemModelo;
                var listaUnique = ListarExperimentoDaAnalise(IdAnalise);

                foreach (var item in listaUnique)
                {
                    itemModelo = new SujeitosPorExperimentoViewModel
                    {
                        NomeExperimento = contexto.Set<Experimento>().Where(exp => exp.Id == item.Value).Select(exp => exp.Nome).FirstOrDefault()
                    };

                    var sujeitos = from med in contexto.Set<Medicao>()
                                   join suj in contexto.Set<SujeitoExperimental>()
                                    on med.SujeitoExperimentalId equals suj.Id
                                   where suj.AnaliseId == IdAnalise & med.ExperimentoId == item.Value
                                   select suj.Descricacao;

                    itemModelo.Sujeitos = sujeitos.Distinct().ToList();
                    modelo.Add(itemModelo);
                }
                return modelo;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao levantar Sujeitos por experimento IdAnlise{IdAnalise}. execute: {ex.Message}");
            }

            return modelo;
        }


        public IList<int?> ListarExperimentoDaAnalise(int IdAnalise)
        {
            var todosExperimentos = from med in contexto.Set<Medicao>()
                                    join suj in contexto.Set<SujeitoExperimental>()
                                        on med.SujeitoExperimentalId equals suj.Id
                                    where suj.AnaliseId == IdAnalise & med.AnaliseId != IdAnalise
                                    select med.ExperimentoId;

            return todosExperimentos.Distinct().ToList();
        }


        public IList<int> TodosSujeitoExperimentoAnalise(int IdAnalise, int IdExperimento)
        {
            var sujeitos = from med in contexto.Set<Medicao>()
                           join suj in contexto.Set<SujeitoExperimental>()
                            on med.SujeitoExperimentalId equals suj.Id
                           where suj.AnaliseId == IdAnalise & med.ExperimentoId == IdExperimento
                           select suj.Id;

            return sujeitos.Distinct().ToList();
        }

        public bool AlterarAnalise(Analise analise)
        {
            try
            {
                contexto.Set<Analise>().Update(analise);
                contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao remover analise de código: {analise.Id}");
            }
            return false;
        }

        public async Task DevativarAnalisesDoProjeto(int id)
        {
            var analises = contexto.Set<Analise>().Where(a => a.ProjetoId == id).ToList();

            foreach (var item in analises){item.Ativa = false;}

            contexto.Set<Analise>()
                .UpdateRange(analises);

            await contexto.SaveChangesAsync().ConfigureAwait(false);
        }

        public int ObterIdProjeto(int analiseId)
        {
            return contexto.Set<Analise>().Find(analiseId).ProjetoId;
        }

        public int ConsultarAcesso(string colaboradorId)
        {
            return contexto.Set<ColaboradorAnalise>().Where(cola => cola.UsuarioAplicacaoId == colaboradorId).Select(c => c.Acesso).FirstOrDefault();
        }
    }
}
