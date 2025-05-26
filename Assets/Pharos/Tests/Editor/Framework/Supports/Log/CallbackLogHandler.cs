using System;
using Pharos.Framework;

namespace PharosEditor.Tests.Framework.Supports
{
    internal class CallbackLogHandler : ILogHandler
    {
        internal delegate void LogParamDelegate(LogParams logParams);
        
        public CallbackLogHandler(LogParamDelegate callback)
        {
            Callback = callback;
        }

        public LogParamDelegate Callback { get; }

        public void Log(object source,
            LogLevel level,
            DateTime timestamp,
            object message,
            params object[] messageParameters)
        {
            Callback?.Invoke(new LogParams(source, level, timestamp, message, messageParameters));
        }
    }
}