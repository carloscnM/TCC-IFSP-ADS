using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchData.Portal.Models.ViewModels.Colaborador;

namespace ResearchData.Portal.Data.Repositorio.Colaborador
{
    public interface IColaboradorRepositorio
    {
        DetalheColaboradorViewModel BuscarDetalheColaborador(string colaId, int IdAnalise);
        Task<bool> RemoverColaboradorAnalise(int idAnalise, string ColaId);
        Task RemoverColaboradores(int IdAnalise);
        string ObterNomeColaborador(string email);
        bool SairDaAnalise(string userId, int idAnalise);
    }
}
