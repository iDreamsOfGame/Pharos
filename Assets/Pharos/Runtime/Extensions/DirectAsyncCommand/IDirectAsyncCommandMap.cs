using System;
using Pharos.Common.CommandCenter;

namespace Pharos.Extensions.DirectAsyncCommand
{
    public interface IDirectAsyncCommandMap : IDirectAsyncCommandMapper
    {
        /// <summary>
        /// Gets a value indicating whether the asynchronous commands execution is aborted.
        /// </summary>
        bool IsAborted { get; }
        
        /// <summary>
        /// Aborts the asynchronous commands execution.
        /// </summary>
        /// <param name="abortExecutingCommand">if set to <c>true</c> abort current command execution.</param>
        void Abort(bool abortExecutingCommand = true);

        /// <summary>
        /// Adds a handler to the process mappings
        /// </summary>
        /// <returns>Self</returns>
        /// <param name="processor">Delegate that accepts a mapping</param>
        IDirectAsyncCommandMap AddMappingProcessor(Action<ICommandMapping> processor);
    }
}