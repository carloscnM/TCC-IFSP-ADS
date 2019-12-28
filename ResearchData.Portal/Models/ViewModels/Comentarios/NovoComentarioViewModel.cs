using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Comentarios
{
    public class NovoComentarioViewModel
    {
        public string TextoComentario { get; set; }
        public string UserName { get; set; }
        public int AnaliseId { get; set; }
    }
}
