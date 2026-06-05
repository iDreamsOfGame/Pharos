using System;
using System.Collections.Generic;

namespace Pharos.Framework.Helpers
{
    internal class LogManager : ILogHandler
    {
        private readonly List<ILogHandler> handlers = new();

        public LogLevel LogLevel { get; set; } = LogLevel.Info;

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