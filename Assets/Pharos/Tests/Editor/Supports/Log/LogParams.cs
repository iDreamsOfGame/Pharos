using System;
using Pharos.Framework;

namespace PharosEditor.Tests.Supports
{
    internal struct LogParams
    {
        public LogParams(object source,
            LogLevel level,
            DateTime timestamp,
            object message,
            params object[] messageParameters)
        {
            Source = source;
            Level = level;
            Timestamp = timestamp;
            Message = message;
            MessageParameters = messageParameters;
        }

        public object Source { get; }

        public LogLevel Level { get; }

        public DateTime Timestamp { get; }

        public object Message { get; }

        public object[] MessageParameters { get; }
    }
}