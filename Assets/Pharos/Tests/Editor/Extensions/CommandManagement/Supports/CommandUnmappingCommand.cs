using System;
using Pharos.Common.EventCenter;
using Pharos.Extensions.CommandManagement;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    internal class CommandUnmappingCommand
    {
        [Inject]
        public IEvent Event { get; private set; }

        [Inject("nestedCommand")]
        public Type CommandType { get; private set; }

        [Inject]
        public IEventCommandMap EventCommandMap { get; private set; }

        public void Execute()
        {
            EventCommandMap.Unmap(Event.EventType, typeof(Event)).FromCommand(CommandType);
        }
    }
}