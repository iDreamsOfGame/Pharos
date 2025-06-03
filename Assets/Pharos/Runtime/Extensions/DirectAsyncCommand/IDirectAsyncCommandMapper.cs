using System;
using Pharos.Common.CommandCenter;

namespace Pharos.Extensions.DirectAsyncCommand
{
    public interface IDirectAsyncCommandMapper
    {
        /// <summary>
        /// Sets the callback function that remaining commands was aborted. 
        /// </summary>
        /// <param name="callback">The callback function that remaining commands was aborted.</param>
        /// <returns>The command mapper.</returns>
        IDirectAsyncCommandMapper SetCommandsAbortedCallback(Action callback);

        /// <summary>
        /// Sets the callback function that all commands executed. 
        /// </summary>
        /// <param name="callback">The callback function that all commands executed.</param>
        /// <returns>The command mapper.</returns>
        IDirectAsyncCommandMapper SetCommandsExecutedCallback(Action callback);

        /// <summary>
        /// Sets the callback function that single asynchronous command executed. 
        /// </summary>
        /// <param name="callback">The callback function that single asynchronous command executed.</param>
        /// <returns>The command mapper.</returns>
        IDirectAsyncCommandMapper SetCommandExecutedCallback(Action<Type, int, int> callback);
        
        /// <summary>
        /// Creates a mapping for a command class. 
        /// </summary>
        /// <param name="commandType">The concrete asynchronous command type.</param>
        /// <returns>The command configurator.</returns>
        IDirectAsyncCommandConfigurator Map(Type commandType);

        /// <summary>
        /// Creates a mapping for a command class. 
        /// </summary>
        /// <typeparam name="T">The concrete asynchronous command class.</typeparam>
        /// <returns>The command configurator.</returns>
        IDirectAsyncCommandConfigurator Map<T>() where T : IAsyncCommand;
        
        /// <summary>
        /// Executes the configured command(s)
        /// </summary>
        /// <param name="payload">The command payload</param>
        void Execute(CommandPayload payload = default);
    }
}