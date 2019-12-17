using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.Data.Repositorio.Sujeitos;
using ResearchData.Portal.Models.Negocio.Experimentos;
using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Medicoes;
using ResearchData.Portal.Models.ViewModels.Sujeitos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace ResearchData.Portal.Data.Repositorio.Medicoes
{
    public class MedicaoRepositorio : BaseRepositorio<Medicao>, IMedicaoRepositorio
    {

        private readonly ILogger _logger;

        #region construtor
        public MedicaoRepositorio(RDContextoDaAplicacao contexto, ILogger<MedicaoRepositorio> logger) : base(contexto)
        {
            this._logger = logger;
        }


        #endregion

        #region MetodosDeUsoEntreRepositorio

        public IQueryable<RetornoDeMedicoes> RetornarMedicoes(int idSujeito, int? IdExperimento)
        {
            var medicoes = from med in contexto.Set<Medicao>()
                           join car in contexto.Set<Caracteristica>() on med.CaracteristicaId equals car.Id
                           where med.SujeitoExperimentalId == idSujeito & med.ExperimentoId == IdExperimento
                           select new RetornoDeMedicoes()
                           {
                               IdMedicao = med.Id,
                               NomeCaract = car.Descricao,
                               DataCaptacao = med.DataCaptacao,
                               Resultadoint = med.MedInt,
                               ResultadoBool = med.MedBool,
                               ResultadoData = med.MedData.ToString(),
                               ResultadoDouble = Convert.ToDouble(med.MedDouble, System.Globalization.CultureInfo.CurrentCulture).ToString(),
                               ResultadoString = med.MedString,
                               IdExperimento = med.SujeitoExperimentalId,
                               Tipo = car.TipoDoDado
                           };
            return medicoes;
        }

        #endregion


        private IList<MedicoesViewModel> RetornarModeloEdicao(IQueryable<RetornoDeMedicoes> lista)
        {
            IList<MedicoesViewModel> modelo = new List<MedicoesViewModel>();
            MedicoesViewModel itemDoModelo;
            try
            {
                //throw  new FormatException("erro Retornar-Modelo-Edicao");
                if (lista.Count() > 0)
                {

                    foreach (var item in lista)
                    {
                        itemDoModelo = new MedicoesViewModel()
                        {
                            IdMedicao = item.IdMedicao,
                            DescricaoMedicao = item.NomeCaract,
                            DataCaptacao = item.DataCaptacao,
                            IdExperimento = item.IdExperimento,
                            TipoDoDado = item.Tipo
                        };

                        switch (item.Tipo)
                        {
                            case TipoDoDado.tint:
                                if (item.DataCaptacao == null)
                                    itemDoModelo.ResultadoMedicao = "";
                                else
                                    itemDoModelo.ResultadoMedicao = item.Resultadoint.ToString();
                                itemDoModelo.TipoHtml = "number";
                                break;
                            case TipoDoDado.tfloat:
                                if (item.DataCaptacao == null)
                                    itemDoModelo.ResultadoMedicao = "";
                                else
                                    itemDoModelo.ResultadoMedicao = item.ResultadoDouble;
                                itemDoModelo.TipoHtml = "number";
                                break;
                            case TipoDoDado.tstring:
                                itemDoModelo.ResultadoMedicao = item.ResultadoString;
                                itemDoModelo.TipoHtml = "text";
                                break;
                            case TipoDoDado.tdata:
                                itemDoModelo.ResultadoMedicao = item.ResultadoData;
                                if (item.ResultadoData != null)
                                    itemDoModelo.ResultadoMedicao = itemDoModelo.ResultadoMedicao.Substring(0, item.ResultadoData.IndexOf(" "));
                                itemDoModelo.TipoHtml = "date";
                                break;
                            case TipoDoDado.tbool:
                                if (item.ResultadoBool == true)
                                {
                                    itemDoModelo.ResultadoMedicao = "1";
                                    break;
                                }
                                if (item.ResultadoBool == false)
                                {
                                    itemDoModelo.ResultadoMedicao = "0";
                                    break;
                                }
                                itemDoModelo.ResultadoMedicao = "";
                                break;
                        }

                        modelo.Add(itemDoModelo);

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Action RetornarModeloEdicao :: MedicaoRepositorio -> execute: " + ex.ToString());
            }
            return modelo;
        }




        public IList<MedicoesViewModel> MontarListaModeloMedicoes(int idSujeito)
        {
            var medicoes = RetornarMedicoes(idSujeito, null);

            return RetornarModeloEdicao(medicoes);
        }



        public IList<MedicoesViewModel> MontarListaModeloMedicoesExp(int idSujeito, int IdExperimento)
        {
            var medicoes = RetornarMedicoes(idSujeito, IdExperimento);

            return RetornarModeloEdicao(medicoes);
        }


        public string[] BuscarResultadoParaCsv(int idAnalise, int idExperimento, int idSujeito)
        {
            var nomeSujeito = contexto.Set<SujeitoExperimental>().Where(suj => suj.Id == idSujeito).Select(suj => suj.Descricacao).FirstOrDefault();
            var nomeExp = contexto.Set<Experimento>().Where(exp => exp.Id == idExperimento).Select(exp => exp.Nome).FirstOrDefault();
            var grupo = from gru in contexto.Set<Grupo>()
                        join suj in contexto.Set<SujeitoExperimental>()
                            on gru.Id equals suj.GrupoId
                        where suj.Id == idSujeito
                        select gru.Nome;

            IList<string> modelobase = new List<string> { "", nomeSujeito, nomeExp, grupo.FirstOrDefault() };
            

            var medicoes = RetornarMedicoes(idSujeito, idExperimento);

            IList<string> itemModelo = RetornarModeloEdicao(medicoes).OrderBy(obj => obj.IdExperimento).Select(obj => obj.ResultadoMedicao).ToList();

            foreach (var item in itemModelo)
            {
                modelobase.Add(item);
            }

            return modelobase.ToArray();
        }


        public async Task SalvarMedicoesExperimento(int idSujeito, int IdExperimento, MedicoesViewModel item)
        {
            try
            {
                //throw  new FormatException("erro Salvar-Medicoes-Experimento");
                var carId = from med in contexto.Set<Medicao>()
                            where med.Id == item.IdMedicao
                            select med.CaracteristicaId;


                Medicao medicao = new Medicao()
                {
                    Id = item.IdMedicao,
                    ExperimentoId = IdExperimento,
                    SujeitoExperimentalId = idSujeito,
                    DataCaptacao = item.DataCaptacao,
                    DataModificacao = DateTime.Now,
                    CaracteristicaId = carId.FirstOrDefault()
                };

                if (item.DataCaptacao == null && (item.ResultadoMedicao != null || !item.ResultadoMedicao.Equals("")))
                    medicao.DataCaptacao = DateTime.Now;

                switch (item.TipoDoDado)
                {
                    case TipoDoDado.tint:
                        medicao.MedInt = Convert.ToInt32(item.ResultadoMedicao);
                        break;
                    case TipoDoDado.tfloat:
                        medicao.MedDouble = Convert.ToDouble(item.ResultadoMedicao, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case TipoDoDado.tstring:
                        medicao.MedString = item.ResultadoMedicao;
                        break;
                    case TipoDoDado.tdata:
                        medicao.MedData = Convert.ToDateTime(item.ResultadoMedicao);
                        break;
                    case TipoDoDado.tbool:
                        if (item.ResultadoMedicao == "1")
                        {
                            medicao.MedBool = true;
                            break;
                        }
                        if (item.ResultadoMedicao == "0")
                        {
                            medicao.MedBool = false;
                            break;
                        }
                        medicao.MedBool = null;
                        break;
                }

                contexto.Set<Medicao>().Update(medicao);
                await contexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Action SalvarMedicoesExperimento :: MedicaoRepositorio -> execute: " + ex.ToString());
            }
        }



        public async Task SalvarMedicoesComuns(int idSujeito, int idAnalise, MedicoesViewModel item)
        {
            try
            {
                //throw  new FormatException("erro Salvar-Medicoes-Comuns");
                var carId = from med in contexto.Set<Medicao>()
                            where med.Id == item.IdMedicao
                            select med.CaracteristicaId;


                Medicao medicao = new Medicao()
                {
                    Id = item.IdMedicao,
                    AnaliseId = idAnalise,
                    SujeitoExperimentalId = idSujeito,
                    DataCaptacao = item.DataCaptacao,
                    DataModificacao = DateTime.Now,
                    CaracteristicaId = carId.FirstOrDefault()
                };

                switch (item.TipoDoDado)
                {
                    case TipoDoDado.tint:
                        medicao.MedInt = Convert.ToInt32(item.ResultadoMedicao);
                        break;
                    case TipoDoDado.tfloat:
                        medicao.MedDouble = Convert.ToDouble(item.ResultadoMedicao);
                        break;
                    case TipoDoDado.tstring:
                        medicao.MedString = item.ResultadoMedicao;
                        break;
                    case TipoDoDado.tdata:
                        medicao.MedData = Convert.ToDateTime(item.ResultadoMedicao);
                        break;
                    case TipoDoDado.tbool:
                        if (item.ResultadoMedicao == "1")
                        {
                            medicao.MedBool = true;
                            break;
                        }
                        if (item.ResultadoMedicao == "0")
                        {
                            medicao.MedBool = false;
                            break;
                        }
                        medicao.MedBool = null;
                        break;
                }

                contexto.Set<Medicao>().Update(medicao);
                await contexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Action SalvarMedicoesComuns :: MedicaoRepositorio -> execute: " + ex.ToString());
            }
        }

        public IList<ListaSujeitoSimplesViewModel> ObterSujeitosNoExperimento(int idAnalise, int idExperimento)
        {
            var sujeitos = from suj in contexto.Set<SujeitoExperimental>()
                           join med in contexto.Set<Medicao>() on suj.Id equals med.SujeitoExperimentalId
                           where suj.AnaliseId == idAnalise & med.ExperimentoId == idExperimento
                           select new ListaSujeitoSimplesViewModel()
                           {
                               IdSujeito = suj.Id,
                               Descricao = suj.Descricacao,
                               IsChecked = true
                           };

            sujeitos = sujeitos.Distinct();

            return sujeitos.ToList();
        }

        public IList<ListaSujeitoSimplesViewModel> ObterSujeitosDisponiveisExperimento(int idAnalise, int idExperimento)
        {
            var sujeitosId = from su in contexto.Set<SujeitoExperimental>()
                             join med in contexto.Set<Medicao>() on su.Id equals med.SujeitoExperimentalId
                             where med.ExperimentoId == idExperimento
                             select su.Id;

            sujeitosId = sujeitosId.Distinct();

            var sujeitos = from suj in contexto.Set<SujeitoExperimental>()
                           //join med in contexto.Set<Medicao>() on suj.Id equals med.SujeitoExperimentalId
                           where suj.AnaliseId == idAnalise & !sujeitosId.Contains(suj.Id)
                           select new ListaSujeitoSimplesViewModel()
                           {
                               IdSujeito = suj.Id,
                               Descricao = suj.Descricacao,
                               IsChecked = false
                           };

            sujeitos = sujeitos.Distinct();

            return sujeitos.ToList();
        }

        public async Task AddMedicaoExperimental(int idExperimento, int idSujeito)
        {
            var listaCar = from gruDad in contexto.Set<TemplateExperimento>()
                           where gruDad.ExperimentoId == idExperimento
                           select gruDad.CaracteristicaId;


            if (listaCar.Count() > 0)
            {
                Medicao medicao;

                foreach (var item in listaCar)
                {
                    medicao = new Medicao()
                    {
                        SujeitoExperimentalId = idSujeito,
                        CaracteristicaId = item,
                        ExperimentoId = idExperimento
                    };
                    await contexto.Set<Medicao>().AddAsync(medicao);
                }
                await contexto.SaveChangesAsync();
            }
        }

        public async Task RemoveMedicaoExperimental(int idExperimento, int idSujeito)
        {
            var medicoes = from med in contexto.Set<Medicao>()
                           where med.ExperimentoId == idExperimento & med.SujeitoExperimentalId == idSujeito
                           select med;

            contexto.Set<Medicao>().RemoveRange(medicoes);
            await contexto.SaveChangesAsync();
        }

        
    }
}
