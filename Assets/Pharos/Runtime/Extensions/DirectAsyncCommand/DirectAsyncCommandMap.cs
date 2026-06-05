using System;
using System.Collections.Generic;
using Pharos.Common.CommandCenter;
using Pharos.Framework;
using Pharos.Framework.Injection;
using VContainer;

namespace Pharos.Extensions.DirectAsyncCommand
{
    public class DirectAsyncCommandMap : IDirectAsyncCommandMap
    {
        private readonly List<Action<ICommandMapping>> mappingProcessors = new();

        private readonly IContext context;

        private readonly IPharosInjector sandboxInjector;
        
        private readonly ICommandMappingList mappings;
        
        private readonly IAsyncCommandsExecutor executor;

        private Action commandsAbortedCallback;
        
        private Action commandsExecutedCallback;
        
        [Inject]
        public DirectAsyncCommandMap(IContext context)
        {
            this.context = context;
            sandboxInjector = context.Injector.CreateChild();
            sandboxInjector.Map<IDirectAsyncCommandMap>().ToValue(this);
            mappings = new CommandMappingList(NullCommandTrigger.Instance, mappingProcessors, context.GetLogger(this));
            executor = new AsyncCommandsExecutor(context, sandboxInjector, mappings.RemoveMapping);
            executor.SetCommandsAbortedCallback(OnCommandsAbortedCallback);
            executor.SetCommandsExecutedCallback(OnCommandsExecuted);
        }

        public bool IsAborted => executor?.IsAborted ?? false;
        
        public IDirectAsyncCommandMap AddMappingProcessor(Action<ICommandMapping> processor)
        {
            if (!mappingProcessors.Contains(processor))
                mappingProcessors.Add(processor);

            return this;
        }
        
        public IDirectAsyncCommandMapper SetCommandExecutedCallback(Action<Type, int, int> callback)
        {
            executor.SetCommandExecutedCallback(callback);
            return this;
        }

        public IDirectAsyncCommandMapper SetCommandsAbortedCallback(Action callback)
        {
            commandsAbortedCallback = callback;
            return this;
        }

        public IDirectAsyncCommandMapper SetCommandsExecutedCallback(Action callback)
        {
            commandsExecutedCallback = callback;
            return this;
        }
        
        public IDirectAsyncCommandConfigurator Map(Type commandType)
        {
            return new DirectAsyncCommandMapper(executor, mappings, commandType);
        }

        public IDirectAsyncCommandConfigurator Map<T>() where T : IAsyncCommand
        {
            return Map(typeof(T));
        }
        
        public void Execute(CommandPayload payload = default)
        {
            executor.ExecuteCommands(mappings.Mappings, payload);
        }

        public void Abort(bool abortExecutingCommand = true)
        {
            executor.Abort(abortExecutingCommand);
        }

        private void OnCommandsAbortedCallback()
        {
            commandsAbortedCallback?.Invoke();
            Dispose();
        }

        private void OnCommandsExecuted()
        {
            commandsExecutedCallback?.Invoke();
            Dispose();
        }

        private void Dispose()
        {
            context.Injector.RemoveChild(sandboxInjector);
        }
    }
}