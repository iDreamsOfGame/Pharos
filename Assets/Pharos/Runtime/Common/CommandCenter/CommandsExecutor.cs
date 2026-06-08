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

        public CommandsExecutor(Action<ICommandMapping> removeMappingProcessor = null, 
            Action<ICommand, ICommandMapping> executingPreprocessor = null, 
            Action<ICommand, ICommandMapping> resultHandler = null)
        {
            this.removeMappingProcessor = removeMappingProcessor;
            this.executingPreprocessor = executingPreprocessor;
            this.resultHandler = resultHandler;
        }

        public void ExecuteCommand(IPharosInjector injector, ICommandMapping mapping, CommandPayload payload = default)
        {
            if (mapping == null)
                return;

            var sandboxInjector = injector.CreateChild();
            var hasPayload = payload.HasPayload;
            var payloadInjectable = hasPayload && mapping.PayloadInjectable;

            ICommand command = null;

            // Inject payload.
            if (payloadInjectable)
                MapPayload(sandboxInjector, payload);

            // Approve guards and hook hooks.
            if (mapping.GuardTypes == null || mapping.GuardTypes.Count == 0 || Guards.Approve(sandboxInjector, mapping.GuardTypes))
            {
                var commandType = mapping.CommandType;
                if (mapping.ShouldExecuteOnce && removeMappingProcessor != null)
                    removeMappingProcessor.Invoke(mapping);

                command = sandboxInjector.GetInstance(commandType) as ICommand;
                if (command == null)
                {
                    command = sandboxInjector.GetOrCreateNewInstance(commandType) as ICommand;
                    sandboxInjector.Map(commandType).ToValue(command);
                }
                
                if (command != null && mapping.HookTypes is { Count: > 0 })
                {
                    Hooks.Hook(sandboxInjector, mapping.HookTypes);
                }
            }

            // Execute command.
            executingPreprocessor?.Invoke(command, mapping);
            injector.RemoveChild(sandboxInjector);
            if (command != null)
            {
                command.Execute();
                resultHandler?.Invoke(command, mapping);
            }
        }

        public void ExecuteCommands(IPharosInjector injector, IEnumerable<ICommandMapping> mappings, CommandPayload payload = default)
        {
            injector.Build(true);
            
            foreach (var mapping in mappings)
            {
                ExecuteCommand(injector, mapping, payload);
            }
        }

        private static void MapPayload(IPharosInjector sandboxInjector, CommandPayload payload)
        {
            foreach (var (obj, type) in payload.ValueToType)
            {
                if (!sandboxInjector.HasMapping(type))
                    sandboxInjector.Map(type).ToValue(obj);
            }

            sandboxInjector.Build();
        }
    }
}