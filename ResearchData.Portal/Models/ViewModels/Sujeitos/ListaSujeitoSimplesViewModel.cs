using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Sujeitos
{
    public class ListaSujeitoSimplesViewModel
    {
        public int IdSujeito { get; set; }
        public string Descricao { get; set; }
        public bool IsChecked { get; set; }
    }
}
