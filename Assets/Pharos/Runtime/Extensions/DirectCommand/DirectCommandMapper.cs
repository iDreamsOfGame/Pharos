using System;
using Pharos.Common.CommandCenter;
using Pharos.Framework.Injection;

namespace Pharos.Extensions.DirectCommand
{
    internal class DirectCommandMapper : IDirectCommandConfigurator
    {
        private readonly IPharosInjector sandboxInjector;

        private readonly ICommandMappingList mappings;

        private readonly ICommandMapping mapping;

        private readonly ICommandsExecutor executor;

        public DirectCommandMapper(IPharosInjector sandboxInjector,
            ICommandsExecutor executor,
            ICommandMappingList mappings,
            Type commandType)
        {
            mapping = new CommandMapping(commandType);
            mapping.ShouldExecuteOnce = true;
            this.sandboxInjector = sandboxInjector;
            this.executor = executor;
            this.mappings = mappings;
            this.mappings.AddMapping(mapping);
        }

        public IDirectCommandConfigurator Map(Type commandType)
        {
            return new DirectCommandMapper(sandboxInjector, executor, mappings, commandType);
        }

        public IDirectCommandConfigurator Map<T>()
        {
            return Map(typeof(T));
        }

        public IDirectCommandConfigurator WithGuards(params Type[] guards)
        {
            mapping.AddGuards(guards);
            return this;
        }

        public IDirectCommandConfigurator WithHooks(params Type[] hooks)
        {
            mapping.AddHooks(hooks);
            return this;
        }

        public IDirectCommandConfigurator WithPayloadInjection(bool value = true)
        {
            mapping.PayloadInjectable = value;
            return this;
        }

        public void Execute(CommandPayload payload = default)
        {
            executor.ExecuteCommands(sandboxInjector, mappings.Mappings, payload);
        }
    }
}