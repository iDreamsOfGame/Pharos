using System;
using System.Collections.Generic;
using Pharos.Framework;
using Pharos.Framework.Injection;

namespace Pharos.Common.CommandCenter
{
    public class AsyncCommandsExecutor : IAsyncCommandsExecutor
    {
        private readonly IContext context;

        private readonly IInjector injector;

        private readonly ICommandsExecutor executor;
        
        private Queue<ICommandMapping> commandMappingQueue;
        
        private int totalCommandCount;
        
        private CommandPayload payload;
        
        private IAsyncCommand executingCommand;
        
        private Action<Type, int, int> commandExecutedCallback;
        
        private Action commandsAbortedCallback;
        
        private Action commandsExecutedCallback;
        
        public AsyncCommandsExecutor(IContext context, 
            IInjector injector,
            Action<ICommandMapping> removeMappingProcessor = null)
        {
            IsAborted = false;
            this.context = context;
            this.injector = injector;
            executor = new CommandsExecutor(injector, removeMappingProcessor, PreprocessAsyncCommandExecuting);
        }
        
        public bool IsAborted { get; private set; }

        public void Abort(bool abortCurrentCommand = true)
        {
            IsAborted = true;
            
            if (abortCurrentCommand && executingCommand != null)
                executingCommand?.Abort();
        }

        public void ExecuteCommands(IEnumerable<ICommandMapping> mappings, CommandPayload payload = default)
        {
            injector.Build(true);
            commandMappingQueue = new Queue<ICommandMapping>(mappings);
            totalCommandCount = commandMappingQueue.Count;
            this.payload = payload;
            ExecuteNextCommand();
        }
        
        public void SetCommandExecutedCallback(Action<Type, int, int> callback)
        {
            commandExecutedCallback = callback;
        }

        public void SetCommandsAbortedCallback(Action callback)
        {
            commandsAbortedCallback = callback;
        }

        public void SetCommandsExecutedCallback(Action callback)
        {
            commandsExecutedCallback = callback;
        }

        private void PreprocessAsyncCommandExecuting(object command, ICommandMapping commandMapping)
        {
            executingCommand = command as IAsyncCommand;
            if (executingCommand != null)
            {
                executingCommand.ExecutedCallback = CommandExecutedCallback;
                context.Detain(executingCommand);
            }
            else
            {
                ExecuteNextCommand();
            }
        }

        private void ExecuteNextCommand()
        {
            while (!IsAborted && commandMappingQueue.Count > 0)
            {
                var mapping = commandMappingQueue.Dequeue();
                if (mapping == null)
                    continue;
                
                executor.ExecuteCommand(mapping, payload);
                return;
            }
            
            if (IsAborted)
            {
                commandMappingQueue.Clear();
                commandsAbortedCallback?.Invoke();
            }
            else if (commandMappingQueue.Count == 0)
            {
                commandsExecutedCallback?.Invoke();
            }
        }

        private void CommandExecutedCallback(IAsyncCommand command, bool stop = false)
        {
            context.Release(command);
            
            var current = totalCommandCount - commandMappingQueue.Count;
            commandExecutedCallback?.Invoke(command.GetType(), current, totalCommandCount);
            
            if (stop)
                Abort(false);
            
            ExecuteNextCommand();
        }
    }
}