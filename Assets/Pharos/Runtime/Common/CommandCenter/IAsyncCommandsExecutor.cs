using System;
using System.Collections.Generic;

namespace Pharos.Common.CommandCenter
{
    public interface IAsyncCommandsExecutor
    {
        /// <summary>
        /// Gets a value indicating whether the asynchronous commands execution is aborted.
        /// </summary>
        bool IsAborted { get; }
        
        /// <summary>
        /// Aborts asynchronous Command execution.
        /// </summary>
        /// <param name="abortCurrentCommand">if set to <c>true</c> abort current command execution.</param>
        void Abort(bool abortCurrentCommand = true);
        
        /// <summary>
        /// Execute a list of asynchronous commands for a given list of mappings
        /// </summary>
        /// <param name="mappings">The Command Mappings</param>
        /// <param name="payload">The Command Payload</param>
        void ExecuteCommands(IEnumerable<ICommandMapping> mappings, CommandPayload payload = default);

        /// <summary>
        /// Sets the callback function that remaining commands was aborted.
        /// </summary>
        /// <param name="callback">The callback function that remaining commands was aborted.</param>
        void SetCommandsAbortedCallback(Action callback);

        /// <summary>
        /// Sets the callback function that all commands executed.
        /// </summary>
        /// <param name="callback">The callback function that all commands executed to invoke.</param>
        void SetCommandsExecutedCallback(Action callback);
        
        /// <summary>
        /// Sets the callback function that single asynchronous command executed.
        /// </summary>
        /// <param name="callback">The callback function that single asynchronous command executed.</param>
        void SetCommandExecutedCallback(Action<Type, int, int> callback);
    }
}