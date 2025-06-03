using System;
using System.Collections.Generic;
using Pharos.Common.CommandCenter;
using Pharos.Framework;

namespace Pharos.Extensions.DirectAsyncCommand
{
    public class DirectAsyncCommandMap : IDirectAsyncCommandMap
    {
        private readonly List<Action<ICommandMapping>> mappingProcessors = new();
        
        private readonly ICommandMappingList mappings;
        
        private readonly IAsyncCommandsExecutor executor;
        
        public DirectAsyncCommandMap(IContext context)
        {
            var childInjector = context.Injector.CreateChild();
            childInjector.Map<IDirectAsyncCommandMap>().ToValue(this);
            mappings = new CommandMappingList(NullCommandTrigger.Instance, mappingProcessors, context.GetLogger(this));
            executor = new AsyncCommandsExecutor(context, childInjector, mappings.RemoveMapping);
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
            executor.SetCommandsAbortedCallback(callback);
            return this;
        }

        public IDirectAsyncCommandMapper SetCommandsExecutedCallback(Action callback)
        {
            executor.SetCommandsExecutedCallback(callback);
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
    }
}