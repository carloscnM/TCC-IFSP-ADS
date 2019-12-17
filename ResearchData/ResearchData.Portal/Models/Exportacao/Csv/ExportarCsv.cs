using ResearchData.Portal.Data.Repositorio.Analises;
using ResearchData.Portal.Data.Repositorio.Experimentos;
using ResearchData.Portal.Data.Repositorio.Medicoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.Exportacao
{
    public class ExportarCsv : IExportarCsv
    {
        private IAnaliseRepositorio _analiseRepo;
        private IExperimentoRepositorio _repoExperi;
        private IMedicaoRepositorio _repoMed;

        public ExportarCsv(IAnaliseRepositorio AnaliseRepo,
                                IExperimentoRepositorio experimentoRepositorio,
                                    IMedicaoRepositorio medicaoRepositorio)
        {
            this._analiseRepo = AnaliseRepo;
            this._repoExperi = experimentoRepositorio;
            this._repoMed = medicaoRepositorio;
        }


        public string RetornarDadosAnaliticoAnalise(int IdAnalise)
        {
            var builder = new StringBuilder();
            try
            {
                var ListaExperimento = _analiseRepo.ListarExperimentoDaAnalise(IdAnalise);
                if(ListaExperimento.Count > 0)
                {
                    int i = 1;
                    foreach (var item in ListaExperimento)
                    {
                        var nomeColunas = BuscarCabecalhoExperimento(item.Value, i);
                        i++;
                        builder.AppendJoin(";", nomeColunas);
                        builder.AppendLine();
                        foreach (var linhas in BuscarDadosExperimento(IdAnalise, item.Value))
                        {
                            builder.AppendJoin(";", linhas);
                            builder.AppendLine();
                        }
                    }
                    return builder.ToString();
                }

                new ArgumentNullException("Sem dados para geração");             
            }
            catch (ArgumentNullException)
            {
                
            }
            return builder.ToString();
        }


        private string[] BuscarCabecalhoExperimento(int IdExperimento, int cont)
        {
            IList<string> abase = new List<string>() { $"{cont}", "Sujeito", "Experimento", "Grupo" };
            IList<string> cabecalhoExperimento = _repoExperi.BuscarNomeCaracteriscaExp(IdExperimento);

            foreach (var item in cabecalhoExperimento)
            {
                abase.Add(item);
            }

            string[] modelo = abase.ToArray();
            return modelo;
        }

        private IList<string[]> BuscarDadosExperimento(int IdAnalise, int IdExperimento)
        {
            IList<string[]> modelo = new List<string[]>();
            var todosSujeitosExp = _analiseRepo.TodosSujeitoExperimentoAnalise(IdAnalise, IdExperimento);

            foreach (var item in todosSujeitosExp)
            {
                var auxiliar = _repoMed.BuscarResultadoParaCsv(IdAnalise, IdExperimento, item);
                
                modelo.Add(auxiliar);
            }
            return modelo;
        }

    }
}
