using System;
using System.Collections.Generic;
using Pharos.Framework.Helpers;
using Pharos.Framework.Injection;

namespace Pharos.Common.CommandCenter
{
    public class CommandsExecutor : ICommandsExecutor
    {
        private readonly IInjector injector;
        
        private readonly Action<ICommandMapping> removeMappingProcessor;

        private readonly Action<ICommand, ICommandMapping> executingPreprocessor;

        private readonly Action<ICommand, ICommandMapping> resultHandler;
        
        private readonly Action completedHandler;

        public CommandsExecutor(IInjector injector,
            Action<ICommandMapping> removeMappingProcessor = null, 
            Action<ICommand, ICommandMapping> executingPreprocessor = null, 
            Action<ICommand, ICommandMapping> resultHandler = null, 
            Action completedHandler = null)
        {
            this.injector = injector;
            this.removeMappingProcessor = removeMappingProcessor;
            this.executingPreprocessor = executingPreprocessor;
            this.resultHandler = resultHandler;
            this.completedHandler = completedHandler;
        }

        public void ExecuteCommand(ICommandMapping mapping, CommandPayload payload = default)
        {
            if (mapping == null)
                return;

            var hasPayload = payload.HasPayload;
            var payloadInjectable = hasPayload && mapping.PayloadInjectable;

            ICommand command = null;

            // Inject payload.
            if (payloadInjectable)
                MapPayload(payload);

            // Approve guards and hook hooks.
            if (mapping.GuardTypes == null || mapping.GuardTypes.Count == 0 || Guards.Approve(injector, mapping.GuardTypes))
            {
                var commandType = mapping.CommandType;
                if (mapping.ShouldExecuteOnce && removeMappingProcessor != null)
                    removeMappingProcessor.Invoke(mapping);

                command = injector.GetInstance(commandType) as ICommand;
                if (command == null)
                {
                    injector.Map(commandType).AsScopedType();
                    injector.Build();
                    command = injector.GetInstance(commandType) as ICommand;
                }
                
                if (command != null && mapping.HookTypes is { Count: > 0 })
                {
                    Hooks.Hook(injector, mapping.HookTypes);
                }
            }

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
            injector.Build(true);
            
            foreach (var mapping in mappings)
            {
                ExecuteCommand(mapping, payload);
            }
            
            completedHandler?.Invoke();
        }

        private void MapPayload(CommandPayload payload)
        {
            foreach (var (obj, type) in payload.ValueToType)
            {
                if (!injector.HasMapping(type))
                    injector.Map(type).ToValue(obj);
            }

            injector.Build();
        }
    }
}