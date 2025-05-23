using System;

namespace Pharos.Framework
{
    /// <summary>
    /// Interface for custom log handler implementation.
    /// </summary>
    public interface ILogHandler
    {
        /// <summary>
        /// Captures a log message. 
        /// </summary>
        /// <param name="source">The source of the log message. </param>
        /// <param name="level">The log level of the message. </param>
        /// <param name="timestamp">message timestamp. </param>
        /// <param name="message">The log message. </param>
        /// <param name="messageParameters">The message parameters. </param>
        void Log(object source,
            LogLevel level,
            DateTime timestamp,
            object message,
            params object[] messageParameters);
    }
}