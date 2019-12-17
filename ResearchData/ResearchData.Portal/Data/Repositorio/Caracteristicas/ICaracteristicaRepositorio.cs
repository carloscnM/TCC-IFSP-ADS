
using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Caracteristicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio.Caracteristicas
{
    public interface ICaracteristicaRepositorio
    {
        Task<bool> AddCaracteristica(Caracteristica caracteristica);
        bool ExisteCaracteristica(string descricao, TipoDoDado tipo, bool comum);
        Task RemoverCaracteristicaComunAnalise(int idAnalise, int caracteristicaId);
        Task AdicionarCaracteriticaAnalise(int idAnalise, int caracteristicaId);


        IList<ListaDeCaracteristicasViewModel> ListarTodasCaracterisca(bool comum);
        IList<ListaDeCaracteristicasViewModel> ListarCaracteriscasSelecionadas(int idAnalise);
        IList<ListaDeCaracteristicasViewModel> ListaCaracteristicaDisponiveis(int idAnalise);
        IList<ListaDeCaracteristicasViewModel> ListaTodasCaracteristicaEspecificas();



    }
}
