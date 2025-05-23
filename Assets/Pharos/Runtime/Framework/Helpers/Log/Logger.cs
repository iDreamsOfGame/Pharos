using System;

namespace Pharos.Framework.Helpers
{
    /// <summary>
    /// Default framework logger.
    /// </summary>
    public class Logger : ILogger
    {
        public Logger(object source, ILogHandler handler)
        {
            Source = source;
            Handler = handler;
        }
        
        /// <summary>
        /// The source of log messages. 
        /// </summary>
        public object Source { get; }

        /// <summary>
        /// The log handler instance. 
        /// </summary>
        public ILogHandler Handler { get; }

        public void LogDebug(object message, params object[] parameters)
        {
            Handler?.Log(Source, LogLevel.Debug, DateTime.Now, message, parameters);
        }

        public void LogInfo(object message, params object[] parameters)
        {
            Handler?.Log(Source, LogLevel.Info, DateTime.Now, message, parameters);
        }

        public void LogWarning(object message, params object[] parameters)
        {
            Handler?.Log(Source, LogLevel.Warning, DateTime.Now, message, parameters);
        }

        public void LogError(object message, params object[] parameters)
        {
            Handler?.Log(Source, LogLevel.Error, DateTime.Now, message, parameters);
        }

        public void LogFatalError(object message, params object[] parameters)
        {
            Handler?.Log(Source, LogLevel.FatalError, DateTime.Now, message, parameters);
        }
    }
}