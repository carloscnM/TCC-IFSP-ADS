using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.Data.Repositorio.Medicoes;
using ResearchData.Portal.Models.Negocio.Experimentos;
using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Experimentos;
using ResearchData.Portal.Models.ViewModels.Sujeitos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ResearchData.Portal.Models.Negocio.Analises;

namespace ResearchData.Portal.Data.Repositorio.Sujeitos
{
    public class SujeitoRepositorio : BaseRepositorio<SujeitoExperimental>, ISujeitoRepositorio
    {
        private IMedicaoRepositorio _repoMedicao;
        private readonly ILogger _logger;

        public SujeitoRepositorio(RDContextoDaAplicacao contexto, ILogger<SujeitoRepositorio> logger, IMedicaoRepositorio repoMedicao) : base(contexto)
        {
            this._repoMedicao = repoMedicao;
            this._logger = logger;
        }

        public string BuscarDescricaoPorId(int idSujeito)
        {
            var descricao = from su in contexto.Set<SujeitoExperimental>()
                            where su.Id == idSujeito
                            select su.Descricacao;

            return descricao.FirstOrDefault();
        }



        public async Task<IEnumerable<ListaSujeitosViewModel>> BuscarSujeitosAnalise(int IdAnalise, int IdProjeto)
        {
            var listaSujeito = from su in contexto.Set<SujeitoExperimental>()
                               join gru in contexto.Set<Grupo>() on su.GrupoId equals gru.Id
                               where su.AnaliseId == IdAnalise
                               select new ListaSujeitosViewModel()
                               {
                                   Id = su.Id,
                                   Descricao = su.Descricacao,
                                   IdAnalise = su.AnaliseId,
                                   IdProjeto = IdProjeto,
                                   GrupoId = gru.Id,
                                   GrupoNome = gru.Nome
                               };

            return listaSujeito;
        }


        private IList<MedicaoComunsViewModel> RetornaModeloVisualizar(IQueryable<RetornoDeMedicoes> medicoes)
        {
            IList<MedicaoComunsViewModel> modelo = new List<MedicaoComunsViewModel>();
            MedicaoComunsViewModel itemDoModelo;

            if (medicoes.Count() > 0)
            {

                foreach (var item in medicoes)
                {
                    itemDoModelo = new MedicaoComunsViewModel()
                    {
                        Id = item.IdMedicao,
                        CaractDescricao = item.NomeCaract,
                        DataCaptacao = item.DataCaptacao.ToString()
                    };

                    if (item.DataCaptacao != null)
                        itemDoModelo.DataCaptacao = itemDoModelo.DataCaptacao.Substring(0, itemDoModelo.DataCaptacao.IndexOf(" "));


                    switch (item.Tipo)
                    {
                        case TipoDoDado.tint:
                            if (item.DataCaptacao == null)
                                itemDoModelo.MedicaoResultado = "";
                            else
                                itemDoModelo.MedicaoResultado = item.Resultadoint.ToString();
                            break;
                        case TipoDoDado.tfloat:
                            if (item.DataCaptacao == null)
                                itemDoModelo.MedicaoResultado = "";
                            else
                                itemDoModelo.MedicaoResultado = item.ResultadoDouble;
                            break;
                        case TipoDoDado.tstring:
                            itemDoModelo.MedicaoResultado = item.ResultadoString;
                            break;
                        case TipoDoDado.tdata:
                            itemDoModelo.MedicaoResultado = item.ResultadoData;
                            if (item.ResultadoData != null)
                                itemDoModelo.MedicaoResultado = item.ResultadoData.Substring(0, item.ResultadoData.IndexOf(" "));
                            break;
                        case TipoDoDado.tbool:
                            if (item.ResultadoBool == true)
                            {
                                itemDoModelo.MedicaoResultado = "Sim";
                                break;
                            }
                            if (item.ResultadoBool == false)
                            {
                                itemDoModelo.MedicaoResultado = "Não";
                                break;
                            }
                            itemDoModelo.MedicaoResultado = "";
                            break;
                    }

                    modelo.Add(itemDoModelo);

                }
            }

            return modelo;
        }


        public IEnumerable<MedicaoComunsViewModel> BuscarMedicaoComunDoSujeito(int idSujeito)
        {
            var medicoes = _repoMedicao.RetornarMedicoes(idSujeito, null);
            return RetornaModeloVisualizar(medicoes);
        }

        public IEnumerable<MedicaoComunsViewModel> BuscarMedicaoComunDoSujExperimento(int IdSujeito, int IdExperimento)
        {
            var medicoes = _repoMedicao.RetornarMedicoes(IdSujeito, IdExperimento);
            return RetornaModeloVisualizar(medicoes);
        }



        public IEnumerable<ListaSujeitoSimplesViewModel> ListarSujeitosAnaliseSimples(int idAnalise)
        {
            var sujeitos = from su in contexto.Set<SujeitoExperimental>()
                           where su.AnaliseId == idAnalise
                           select new ListaSujeitoSimplesViewModel()
                           {
                               IdSujeito = su.Id,
                               Descricao = su.Descricacao
                           };

            return sujeitos;
        }

        public async Task<bool> CadastrarSujeitoAnalise(SujeitoExperimental sujeito)
        {
            try
            {
                //throw  new FormatException("erro Cadastrar-Sujeito-Analise");
                await contexto.Set<SujeitoExperimental>().AddAsync(sujeito).ConfigureAwait(false);


                var carParaAdd = from med in contexto.Set<Medicao>()
                                 where med.AnaliseId == sujeito.AnaliseId
                                 select med.CaracteristicaId;



                if (carParaAdd.Count() > 0)
                {
                    var carUnicas = carParaAdd.Distinct();

                    Medicao medicao = new Medicao();

                    foreach (var item in carUnicas)
                    {
                        medicao = new Medicao()
                        {
                            DataModificacao = DateTime.Now,
                            AnaliseId = sujeito.AnaliseId,
                            CaracteristicaId = item,
                            SujeitoExperimentalId = sujeito.Id
                        };

                        await contexto.AddAsync(medicao).ConfigureAwait(false);
                    }
                }


                await contexto.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Action CadastrarSujeitoAnalise :: SujeitoRepositorio -> execute: " + ex.ToString());
                return false;
            }
        }

        public IList<ExpSimplesViewModel> BuscarExpDoSujeito(int idSujeito)
        {
            var experimentosId = from med in contexto.Set<Medicao>()
                                 where med.SujeitoExperimentalId == idSujeito
                                 select med.ExperimentoId;

            experimentosId = experimentosId.Distinct();

            var listaexp = from exp in contexto.Set<Experimento>()
                           where experimentosId.Contains(exp.Id)
                           select new ExpSimplesViewModel()
                           {
                               Id = exp.Id,
                               Nome = exp.Nome
                           };

            return listaexp.ToList();
        }

        public bool VerificarAnaliseAtiva(int idAnalise)
        {
            var encerramentoAnalise = contexto.Set<Analise>()
                                        .Where(ana => ana.Id == idAnalise)
                                            .Select(ana => ana.DataFim);
            if (encerramentoAnalise.FirstOrDefault() == null)
                return true;

            return false;
        }

        public NovoSujeitoViewModel ObterDetalheSujeito(int idSujeito)
        {
            var modelo = from su in contexto.Set<SujeitoExperimental>()
                         join gru in contexto.Set<Grupo>() on su.GrupoId equals gru.Id
                         where su.Id == idSujeito
                         select new NovoSujeitoViewModel()
                         {
                             Id = su.Id,
                             Descricao = su.Descricacao,
                             AnaliseId = su.AnaliseId,
                             GrupoId = su.GrupoId,
                             GrupoNome = gru.Nome
                         };

            return modelo.FirstOrDefault();
        }

        public async Task<bool> EditarSujeito(NovoSujeitoViewModel modelo)
        {
            try
            {
                SujeitoExperimental sujeito = await contexto.Set<SujeitoExperimental>().FindAsync(modelo.Id);
                sujeito.Descricacao = modelo.Descricao;
                sujeito.GrupoId = modelo.GrupoId;
                contexto.Set<SujeitoExperimental>().Update(sujeito);
                await contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoverSujeito(NovoSujeitoViewModel modelo)
        {
            try
            {
                SujeitoExperimental sujeito = await contexto.Set<SujeitoExperimental>().FindAsync(modelo.Id);

                //removendo todos as medições
                contexto.Set<Medicao>().RemoveRange(contexto.Set<Medicao>().Where(med => med.SujeitoExperimentalId == sujeito.Id));
                contexto.Set<SujeitoExperimental>().Remove(sujeito);

                await contexto.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IList<ListaSujeitoSimplesViewModel> BuscarSujeitoExistente(int idAnalise, int idProjeto)
        {
            var sujeitoExistes = contexto.Set<SujeitoExperimental>().Where(su => su.AnaliseId == idAnalise & su.Externo != null).Select(su => su.Externo);


            var modelo = from suj in contexto.Set<SujeitoExperimental>()
                         join gru in contexto.Set<Grupo>() on suj.GrupoId equals gru.Id
                         where gru.ProjetoId == idProjeto & suj.AnaliseId != idAnalise & suj.Externo == null & !sujeitoExistes.Contains(suj.Id)
                         select new ListaSujeitoSimplesViewModel()
                         {
                             IdSujeito = suj.Id,
                             Descricao = suj.Descricacao
                         };

            return modelo.ToList();
        }
    }


}
