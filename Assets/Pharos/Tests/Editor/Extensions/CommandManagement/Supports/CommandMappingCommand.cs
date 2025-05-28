using System;
using Pharos.Extensions.CommandManagement;
using Pharos.Extensions.EventManagement;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    internal class CommandMappingCommand
    {
        [Inject]
        public IEvent Event { get; private set; }

        [Inject("nestedCommand")]
        public Type CommandType { get; private set; }

        [Inject]
        public IEventCommandMap EventCommandMap { get; private set; }
        
        public void Execute()
        {
            EventCommandMap.Map(Event.EventType, typeof(IEvent)).ToCommand(CommandType);
        }
    }
}