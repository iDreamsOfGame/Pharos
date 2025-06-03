using System;
using Pharos.Common.CommandCenter;

namespace Pharos.Extensions.DirectAsyncCommand
{
    internal class DirectAsyncCommandMapper : IDirectAsyncCommandConfigurator
    {
        private readonly IAsyncCommandsExecutor executor;
        
        private readonly ICommandMappingList mappings;

        private readonly ICommandMapping mapping;
        
        public DirectAsyncCommandMapper(IAsyncCommandsExecutor executor, ICommandMappingList mappings, Type commandType)
        {
            this.executor = executor;
            this.mappings = mappings;
            mapping = new CommandMapping(commandType);
            mapping.ShouldExecuteOnce = true;
            this.mappings.AddMapping(mapping);
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
        
        public IDirectAsyncCommandConfigurator WithGuards(params object[] guards)
        {
            mapping.AddGuards(guards);
            return this;
        }

        public IDirectAsyncCommandConfigurator WithHooks(params object[] hooks)
        {
            mapping.AddHooks(hooks);
            return this;
        }

        public IDirectAsyncCommandConfigurator WithPayloadInjection(bool value = true)
        {
            mapping.PayloadInjectionEnabled = value;
            return this;
        }
        
        public void Execute(CommandPayload payload = default)
        {
            executor.ExecuteCommands(mappings.Mappings, payload);
        }
    }
}