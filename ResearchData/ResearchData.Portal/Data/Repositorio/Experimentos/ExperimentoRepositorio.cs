using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.Models.Negocio.Experimentos;
using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Caracteristicas;
using ResearchData.Portal.Models.ViewModels.Experimentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio.Experimentos
{
    public class ExperimentoRepositorio : BaseRepositorio<Experimento>, IExperimentoRepositorio
    {
        public ExperimentoRepositorio(RDContextoDaAplicacao contexto) : base(contexto)
        {

        }

        public async Task<int> AddExperimento(Experimento experimento)
        {
            await contexto.Set<Experimento>().AddAsync(experimento);
            await contexto.SaveChangesAsync();
            return experimento.Id;
        }

        public async Task AddGrupoDeDadosParaExp(int idExperimento, int caracteristicaId)
        {
            TemplateExperimento grupoDeDados = new TemplateExperimento()
            {
                ExperimentoId = idExperimento,
                CaracteristicaId = caracteristicaId
            };

            await contexto.Set<TemplateExperimento>().AddAsync(grupoDeDados);
            await contexto.SaveChangesAsync();
        }

        public IList<ExpSimplesViewModel> BuscarExperimentos()
        {
            var experimentos = from exp in contexto.Set<Experimento>()
                               select new ExpSimplesViewModel()
                               {
                                   Id = exp.Id,
                                   Nome = exp.Nome
                               };
            return experimentos.ToList();
        }

        public IList<ListaDeCaracteristicasViewModel> BuscarGrupoDeDados(int idExperimento)
        {
            var grupoDeDados = from grupo in contexto.Set<TemplateExperimento>()
                               join car in contexto.Set<Caracteristica>() on grupo.CaracteristicaId equals car.Id
                               where grupo.ExperimentoId == idExperimento
                               select new ListaDeCaracteristicasViewModel()
                               {
                                   Descricao = car.Descricao,
                                   Tipo = car.TipoDoDado
                               };

            return grupoDeDados.ToList();
        }

        public IList<string> BuscarNomeCaracteriscaExp(int idExperimento)
        {
            var nomes = from gruDad in contexto.Set<TemplateExperimento>()
                        join car in contexto.Set<Caracteristica>()
                            on gruDad.CaracteristicaId equals car.Id
                        where gruDad.ExperimentoId == idExperimento
                        select new { Nomes = car.Descricao, ExpId = gruDad.ExperimentoId};

            return nomes.OrderBy(exp => exp.ExpId).Select(exp => exp.Nomes).ToList();
        }

        public string ObterDescricaoExperimento(int idExperimento)
        {
            var descricao = from ex in contexto.Set<Experimento>()
                            where ex.Id == idExperimento
                            select ex.Descricao;

            return descricao.FirstOrDefault();
        }

        public async Task<string> ObterNomeExperimento(int idExperimento)
        {
            var experimento = await contexto.Set<Experimento>().FindAsync(idExperimento);
            return experimento.Nome;
        }
    }
}
