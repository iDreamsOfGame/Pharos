using System;
using System.Collections.Generic;
using Pharos.Framework.Helpers;
using Pharos.Framework.Injection;

namespace Pharos.Common.CommandCenter
{
    public class CommandsExecutor : ICommandsExecutor
    {
        private readonly Action<ICommandMapping> removeMappingProcessor;

        private readonly Action<ICommand, ICommandMapping> executingPreprocessor;

        private readonly Action<ICommand, ICommandMapping> resultHandler;

        private readonly IInjector injector;

        public CommandsExecutor(IInjector injector,
            Action<ICommandMapping> removeMappingProcessor = null,
            Action<ICommand, ICommandMapping> executingPreprocessor = null,
            Action<ICommand, ICommandMapping> resultHandler = null)
        {
            this.injector = injector;
            this.removeMappingProcessor = removeMappingProcessor;
            this.executingPreprocessor = executingPreprocessor;
            this.resultHandler = resultHandler;
        }

        public void ExecuteCommand(ICommandMapping mapping, CommandPayload payload = default)
        {
            if (mapping == null)
                return;

            var hasPayload = payload.HasPayload;
            var payloadInjectionEnabled = hasPayload && mapping.PayloadInjectionEnabled;

            ICommand command = null;

            // Inject payload.
            if (payloadInjectionEnabled)
                MapPayload(payload);

            // Approve guards and hook hooks.
            if (mapping.GuardTypes == null || mapping.GuardTypes.Count == 0 || Guards.Approve(injector, mapping.GuardTypes))
            {
                var commandType = mapping.CommandType;
                if (mapping.ShouldExecuteOnce && removeMappingProcessor != null)
                    removeMappingProcessor.Invoke(mapping);
                
                command = injector.GetOrCreateNewInstance(commandType) as ICommand;
                if (command != null && mapping.HookTypes is { Count: > 0 })
                {
                    injector.Map(commandType).ToValue(command);
                    injector.Build();
                    Hooks.Hook(injector, mapping.HookTypes);
                    injector.Unmap(commandType);
                }
            }

            // Uninject payload.
            if (payloadInjectionEnabled)
                UnmapPayload(payload);

            // Execute command.
            executingPreprocessor?.Invoke(command, mapping);
            if (command != null)
            {
                command.Execute();
                resultHandler?.Invoke(command, mapping);
            }
        }

        public void ExecuteCommands(IEnumerable<ICommandMapping> mappings, CommandPayload payload = default)
        {
            injector.Build();
            
            foreach (var mapping in mappings)
            {
                ExecuteCommand(mapping, payload);
            }
        }

        private void MapPayload(CommandPayload payload)
        {
            foreach (var (obj, type) in payload.ValueToType)
            {
                injector.Map(type).ToValue(obj);
            }

            injector.Build();
        }

        private void UnmapPayload(CommandPayload payload)
        {
            foreach (var type in payload.ValueToType.Values)
            {
                injector.Unmap(type);
            }
        }
    }
}