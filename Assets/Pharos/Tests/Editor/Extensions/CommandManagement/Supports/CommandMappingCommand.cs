using System;
using Pharos.Common.EventCenter;
using Pharos.Extensions.CommandManagement;
using VContainer;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    [InjectIgnore]
    internal class CommandMappingCommand
    {
        [Inject]
        public IEvent Event { get; private set; }

        [Inject, Key("nestedCommand")]
        public Type CommandType { get; private set; }

        [Inject]
        public IEventCommandMap EventCommandMap { get; private set; }

        public void Execute()
        {
            EventCommandMap.Map(Event.EventType, typeof(IEvent)).ToCommand(CommandType);
        }
    }
}