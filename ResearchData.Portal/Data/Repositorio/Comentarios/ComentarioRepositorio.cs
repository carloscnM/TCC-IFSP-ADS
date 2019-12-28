using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.Models.Negocio.Analises;
using ResearchData.Portal.Models.ViewModels.Comentarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio.Comentarios
{
    public class ComentarioRepositorio : BaseRepositorio<Comentario>, IComentarioRepositorio
    {
        public ComentarioRepositorio(RDContextoDaAplicacao contexto) : base(contexto)
        {

        }

        public async Task AdicionarComentario(NovoComentarioViewModel modelo)
        {
            try
            {
                var userId = contexto.Users.Where(use => use.UserName == modelo.UserName).Select(use => use.Id);
                Comentario comentario = new Comentario()
                {
                    AnaliseId = modelo.AnaliseId,
                    UsuarioAplicacaoId = userId.FirstOrDefault(),
                    TextoComentario = modelo.TextoComentario,
                    DataInclusao = DateTime.Now
                };
                await contexto.Set<Comentario>().AddAsync(comentario);
                await contexto.SaveChangesAsync();
            }
            catch (Exception)
            {

            }
        }

        public IList<ComentariosViewModel> ListaComenatarioAnalise(int IdAnalise)
        {
            var comentario = from come in contexto.Set<Comentario>()
                             join use in contexto.Users on come.UsuarioAplicacaoId equals use.Id
                             where come.AnaliseId == IdAnalise
                             select new ComentariosViewModel()
                             {
                                 Mensagem = come.TextoComentario,
                                 DataInclusao = come.DataInclusao,
                                 NomeUsuario = use.Nome
                             };
            return comentario.ToList();
        }
    }
}
