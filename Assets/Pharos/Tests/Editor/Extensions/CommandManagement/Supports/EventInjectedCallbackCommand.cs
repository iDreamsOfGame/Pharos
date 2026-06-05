using System;
using Pharos.Common.CommandCenter;
using Pharos.Common.EventCenter;
using VContainer;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    internal class EventInjectedCallbackCommand : ICommand
    {
        [Inject]
        public IEvent Event { get; private set; }

        [Inject, Key("ExecuteCallback")]
        public Action<EventInjectedCallbackCommand> Callback { get; private set; }

        public void Execute()
        {
            Callback?.Invoke(this);
        }
    }
}