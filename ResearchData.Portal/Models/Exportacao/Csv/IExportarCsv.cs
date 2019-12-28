using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.Exportacao
{
    public interface IExportarCsv
    {
        string RetornarDadosAnaliticoAnalise(int IdAnalise);
    }
}
