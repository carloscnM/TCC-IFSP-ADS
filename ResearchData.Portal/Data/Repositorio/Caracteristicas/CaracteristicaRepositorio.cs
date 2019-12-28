using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Caracteristicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio.Caracteristicas
{
    public class CaracteristicaRepositorio : BaseRepositorio<Caracteristica>, ICaracteristicaRepositorio
    {
        public CaracteristicaRepositorio(RDContextoDaAplicacao contexto) : base(contexto)
        {

        }

        public async Task<bool> AddCaracteristica(Caracteristica caracteristica)
        {
            await contexto.Set<Caracteristica>().AddAsync(caracteristica);
            await contexto.SaveChangesAsync();
            return true;
        }



        public IList<ListaDeCaracteristicasViewModel> ListaCaracteristicaDisponiveis(int idAnalise)
        {
            var utilizadas = ListaIdCaracteristicaAnalise(idAnalise);


            var lista = from car in contexto.Set<Caracteristica>()
                        where !utilizadas.Contains(car.Id) & car.Comun == true
                        select new ListaDeCaracteristicasViewModel()
                        {
                            CaracteristicaId = car.Id,
                            Descricao = car.Descricao,
                            Tipo = car.TipoDoDado,
                            IsChecked = false
                        };

            return lista.ToList();
        }





        public IList<ListaDeCaracteristicasViewModel> ListarCaracteriscasSelecionadas(int idAnalise)
        {
            var utilizadas = ListaIdCaracteristicaAnalise(idAnalise);


            var lista = from car in contexto.Set<Caracteristica>()
                        where utilizadas.Contains(car.Id)
                        select new ListaDeCaracteristicasViewModel()
                        {
                            CaracteristicaId = car.Id,
                            Descricao = car.Descricao,
                            Tipo = car.TipoDoDado,
                            IsChecked = true
                        };

            return lista.ToList();
        }

        public IList<ListaDeCaracteristicasViewModel> ListarTodasCaracterisca(bool comum)
        {
            var carateristica = from car in contexto.Set<Caracteristica>()
                                where car.Comun == comum
                                select new ListaDeCaracteristicasViewModel()
                                {
                                    CaracteristicaId = car.Id,
                                    Descricao = car.Descricao,
                                    Tipo = car.TipoDoDado,
                                    IsChecked = false
                                };
            return carateristica.OrderBy(c => c.Descricao).ToList();
        }

        public async Task RemoverCaracteristicaComunAnalise(int idAnalise, int caracteristicaId)
        {
            //pegando a lista de sujeito da análise.
            var medicao = from med in contexto.Set<Medicao>()
                          where med.AnaliseId == idAnalise & med.CaracteristicaId == caracteristicaId
                          select med;


            foreach (var item in medicao)
            {
                contexto.Set<Medicao>().Remove(item);
            }

            await contexto.SaveChangesAsync();
        }

        public async Task AdicionarCaracteriticaAnalise(int idAnalise, int caracteristicaId)
        {
            var listaSujeito = from su in contexto.Set<SujeitoExperimental>()
                               where su.AnaliseId == idAnalise
                               select su.Id;

            if (listaSujeito != null)
            {
                Medicao medicao = new Medicao();

                foreach (var item in listaSujeito)
                {
                    medicao = new Medicao()
                    {
                        DataModificacao = DateTime.Now,
                        AnaliseId = idAnalise,
                        CaracteristicaId = caracteristicaId,
                        SujeitoExperimentalId = item
                    };

                    await contexto.AddAsync(medicao);
                }
                await contexto.SaveChangesAsync();
            }
        }



        private IQueryable<int> ListaIdCaracteristicaAnalise(int id)
        {
            var ids = from med in contexto.Set<Medicao>()
                      where med.AnaliseId == id
                      select med.CaracteristicaId;
            return ids;
        }

        public IList<ListaDeCaracteristicasViewModel> ListaTodasCaracteristicaEspecificas()
        {
            var carateristica = from car in contexto.Set<Caracteristica>()
                                where car.Comun == false
                                select new ListaDeCaracteristicasViewModel()
                                {
                                    CaracteristicaId = car.Id,
                                    Descricao = car.Descricao,
                                    Tipo = car.TipoDoDado,
                                    IsChecked = false
                                };

            return carateristica.ToList();
        }

        public bool ExisteCaracteristica(string descricao, TipoDoDado tipo, bool comum)
        {
            return contexto.Set<Caracteristica>().Where(ca => ca.Descricao == descricao & ca.TipoDoDado == tipo & ca.Comun == comum).Any(); //  .Select(c => c.Id).FirstOrDefault();
        }
    }
}
