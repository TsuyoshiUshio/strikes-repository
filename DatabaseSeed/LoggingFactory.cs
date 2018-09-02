using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DatabaseSeed
{
    class LoggingFactory
    {
        private static readonly ILogger _logger;
        static LoggingFactory()
        {
            ILoggerFactory loggerFactory = new NullLoggerFactory()
                .AddConsole()
                .AddDebug();
            _logger = loggerFactory.CreateLogger("DBSeed");
        }

        public static ILogger Logger => _logger;

    }
}
