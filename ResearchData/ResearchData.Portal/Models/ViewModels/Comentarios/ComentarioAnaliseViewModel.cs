using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Comentarios
{
    public class ComentarioAnaliseViewModel
    {
        public int AnaliseId { get; set; }
        public IList<ComentariosViewModel> Comentarios { get; set; }
    }
}
