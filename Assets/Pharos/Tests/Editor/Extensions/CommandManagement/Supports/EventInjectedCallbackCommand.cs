using System;
using Pharos.Extensions.EventManagement;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    internal class EventInjectedCallbackCommand
    {
        [Inject]
        public IEvent Event { get; private set; }

        [Inject("ExecuteCallback")]
        public Action<EventInjectedCallbackCommand> Callback { get; private set; }

        public void Execute()
        {
            Callback?.Invoke(this);
        }
    }
}