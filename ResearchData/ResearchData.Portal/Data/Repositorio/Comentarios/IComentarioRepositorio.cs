using ResearchData.Portal.Models.ViewModels.Comentarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio.Comentarios
{
    public interface IComentarioRepositorio
    {
        IList<ComentariosViewModel> ListaComenatarioAnalise(int IdAnalise);
        Task AdicionarComentario(NovoComentarioViewModel modelo);
    }
}
