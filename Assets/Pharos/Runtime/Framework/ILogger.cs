namespace Pharos.Framework
{
    /// <summary>
    /// The framework logger contract. 
    /// </summary>
    public interface ILogger
    { 
        /// <summary>
        /// Logs a message for debug purposes. 
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="parameters">Message parameters. </param>
        void LogDebug(object message, params object[] parameters);
        
        /// <summary>
        /// Logs a message for notification purposes. 
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="parameters">Message parameters. </param>
        void LogInfo(object message, params object[] parameters);
        
        /// <summary>
        /// Logs a warning message. 
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="parameters">Message parameters. </param>
        void LogWarning(object message, params object[] parameters);
        
        /// <summary>
        /// Logs an error message. 
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="parameters">Message parameters. </param>
        void LogError(object message, params object[] parameters);
        
        /// <summary>
        /// Logs a fatal error message. 
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="parameters">Message parameters. </param>
        void LogFatalError(object message, params object[] parameters);
    }
}