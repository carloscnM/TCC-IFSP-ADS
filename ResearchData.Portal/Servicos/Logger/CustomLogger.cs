using Microsoft.Extensions.Logging;
using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.Models.Negocio.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Servicos.Logger
{
    public class CustomLogger : ILogger
    {
        readonly string loggerName;
        readonly CustonLoggerProviderConfiguration loggerConfig;
        private readonly RDContextoLog _context;
        public CustomLogger(string name, CustonLoggerProviderConfiguration logger, RDContextoLog context)
        {
            this.loggerName = name;
            this.loggerConfig = logger;
            this._context = context;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string mensagem = String.Format("{0} : {1} - {2}", logLevel.ToString(), eventId.ToString(), formatter(state, exception));
            this.SalvarNoBanco(mensagem);
        }

        public void SalvarNoBanco(string messagem)
        {
            var log = new AppLog
            {
                Mensagem = messagem,
                Captura = DateTime.Now,
            };
            _context.AddAsync(log);
            _context.SaveChangesAsync();
        }
    }
}
