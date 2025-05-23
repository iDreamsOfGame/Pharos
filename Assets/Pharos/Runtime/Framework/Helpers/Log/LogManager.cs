using System;
using System.Collections.Generic;
using ReflexPlus.Logging;

namespace Pharos.Framework.Helpers
{
    internal class LogManager : ILogHandler
    {
        private static readonly Dictionary<LogLevel, ReflexPlus.Logging.LogLevel> LogLevelReflexPlusLogLevelMap = new()
        {
            { LogLevel.FatalError, ReflexPlus.Logging.LogLevel.Error },
            { LogLevel.Error, ReflexPlus.Logging.LogLevel.Error },
            { LogLevel.Warning, ReflexPlus.Logging.LogLevel.Warning },
            { LogLevel.Info, ReflexPlus.Logging.LogLevel.Info },
            { LogLevel.Debug, ReflexPlus.Logging.LogLevel.Development }
        };

        private LogLevel logLevel;

        private readonly List<ILogHandler> handlers = new();

        public LogLevel LogLevel
        {
            get => logLevel;
            set
            {
                logLevel = value;
                if (LogLevelReflexPlusLogLevelMap.TryGetValue(logLevel, out var reflexPlusLogLevel))
                    ReflexPlusLogger.UpdateLogLevel(reflexPlusLogLevel);
            }
        }

        public LogManager()
        {
            LogLevel = LogLevel.Debug;
        }

        public ILogger GetLogger(object source) => new Logger(source, this);

        public void AddLogHandler(ILogHandler handler)
        {
            handlers?.Add(handler);
        }

        public void Log(object source,
            LogLevel level,
            DateTime timestamp,
            object message,
            params object[] messageParameters)
        {
            if (level > LogLevel)
                return;

            foreach (var handler in handlers)
            {
                handler?.Log(source, level, timestamp, message, messageParameters);
            }
        }

        public void Destroy()
        {
            handlers?.Clear();
        }
    }
}