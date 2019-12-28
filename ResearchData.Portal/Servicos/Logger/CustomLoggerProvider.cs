using Microsoft.Extensions.Logging;
using ResearchData.Portal.Data.Contextos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Servicos.Logger
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        readonly CustonLoggerProviderConfiguration loggerConfig;
        readonly ConcurrentDictionary<string, CustomLogger> logger = new ConcurrentDictionary<string, CustomLogger>();

        private RDContextoLog context;
        public RDContextoLog Context { get => context; set => context = value; }


        public CustomLoggerProvider(CustonLoggerProviderConfiguration Config)
        {
            loggerConfig = Config;
        }


        public ILogger CreateLogger(string categoryName)
        {
            return logger.GetOrAdd(categoryName, name => new CustomLogger(name, loggerConfig, Context));
        }

        public void Dispose()
        {
        }
    }
}
