using System;
using System.Collections.Generic;
using System.Linq;
using Pharos.Framework.Helpers;
using Pharos.Framework.Injection;

namespace Pharos.Common.CommandCenter
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly Action<ICommandMapping> removeMappingProcessor;

        private readonly Action<object, ICommandMapping> executingPreprocessor;
        
        private readonly Action<object, object, ICommandMapping> resultHandler;
        
        public CommandExecutor(IInjector injector,
            Action<ICommandMapping> removeMappingProcessor = null,
            Action<object, ICommandMapping> executingPreprocessor = null,
            Action<object, object, ICommandMapping> resultHandler = null)
        {
            Injector = injector;
            this.removeMappingProcessor = removeMappingProcessor;
            this.executingPreprocessor = executingPreprocessor;
            this.resultHandler = resultHandler;
        }
        
        public IInjector Injector { get; }

        public void ExecuteCommand(ICommandMapping mapping, CommandPayload payload = default)
        {
            if (mapping == null)
                return;
            
            var hasPayload = payload.HasPayload;
            var payloadInjectionEnabled = hasPayload && mapping.PayloadInjectionEnabled;
            
            object command = null;
            
            // Inject payload.
            if (payloadInjectionEnabled)
                MapPayload(payload);

            // Approve guards and hook hooks.
            if (mapping.Guards == null || mapping.Guards.Count == 0 || Guards.Approve(Injector, mapping.Guards))
            {
                var commandType = mapping.CommandType;
                if (mapping.ShouldExecuteOnce && removeMappingProcessor != null)
                    removeMappingProcessor.Invoke(mapping);
                
                command = Injector.GetOrCreateNewInstance(commandType);
                if (mapping.Hooks is { Count: > 0 })
                {
                    Injector.Map(commandType).ToValue(command);
                    Injector.Build();
                    Hooks.Hook(Injector, mapping.Hooks);
                    Injector.Unmap(commandType);
                }
            }
            
            // Uninject payload.
            if (payloadInjectionEnabled)
                UnmapPayload(payload);
            
            // Execute command.
            if (command != null)
            {
                executingPreprocessor?.Invoke(command, mapping);
                var executeMethodInfo = mapping.ExecuteMethodInfo;
                if (executeMethodInfo != null)
                {
                    var executeMethodParameters = mapping.ExecuteMethodParameters;
                    var result = hasPayload && executeMethodParameters.Length > 0 
                        ? executeMethodInfo.Invoke(command, payload.ValueTypeMap.Keys.ToArray())
                        : executeMethodInfo.Invoke(command, null);
                    resultHandler?.Invoke(result, command, mapping);
                }
                else
                {
                    throw new MissingMethodException(mapping.CommandType.FullName, nameof(ICommand.Execute));
                }
            }
        }

        public void ExecuteCommands(IEnumerable<ICommandMapping> mappings, CommandPayload payload = default)
        {
            foreach (var mapping in mappings)
            {
                ExecuteCommand(mapping, payload);
            }
        }

        private void MapPayload(CommandPayload payload)
        {
            foreach (var (obj, type) in payload.ValueTypeMap)
            {
                Injector.Map(type).ToValue(obj);
            }
        }

        private void UnmapPayload(CommandPayload payload)
        {
            foreach (var type in payload.ValueTypeMap.Values)
            {
                Injector.Unmap(type);
            }
        }
    }
}