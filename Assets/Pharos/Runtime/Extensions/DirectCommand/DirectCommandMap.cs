using System;
using System.Collections.Generic;
using Pharos.Common.CommandCenter;
using Pharos.Framework;
using Pharos.Framework.Injection;

namespace Pharos.Extensions.DirectCommand
{
    public class DirectCommandMap : IDirectCommandMap
    {
        private readonly List<Action<ICommandMapping>> mappingProcessors = new();

        private readonly IContext context;

        private readonly IInjector sandboxedInjector;

        private readonly ICommandExecutor executor;

        private readonly ICommandMappingList mappings;

        public DirectCommandMap(IContext context)
        {
            this.context = context;
            // sandboxedInjector = context.Injector.CreateChild();
            sandboxedInjector = context.Injector;
            sandboxedInjector.Map<IDirectCommandMap>().ToValue(this);
            mappings = new CommandMappingList(NullCommandTrigger.Instance, mappingProcessors, context.GetLogger(this));
            executor = new CommandExecutor(sandboxedInjector, mappings.RemoveMapping);
        }

        public IDirectCommandConfigurator Map(Type commandType)
        {
            return new DirectCommandMapper(executor, mappings, commandType);
        }

        public IDirectCommandConfigurator Map<T>()
        {
            return Map(typeof(T));
        }

        public void Detain(object command)
        {
            context.Detain(command);
        }

        public void Release(object command)
        {
            context.Release(command);
        }

        public IDirectCommandMap AddMappingProcessor(Action<ICommandMapping> processor)
        {
            if (!mappingProcessors.Contains(processor))
                mappingProcessors.Add(processor);

            return this;
        }

        public void Execute(CommandPayload payload = default)
        {
            executor.ExecuteCommands(mappings.Mappings, payload);
        }
    }
}