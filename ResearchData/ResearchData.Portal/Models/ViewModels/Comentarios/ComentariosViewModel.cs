using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Comentarios
{
    public class ComentariosViewModel
    {
        public string Mensagem { get; set; }
        public string NomeUsuario { get; set; }
        public DateTime DataInclusao { get; set; }
    }
}
