using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Caracteristicas
{
    public class ImportarCaracteristicaViewModel
    {
        [Required]
        public IFormFile ArquivoXml { get; set; }

        public IList<LogImportacaoViewModel> Log { get; set; }
    }
}
