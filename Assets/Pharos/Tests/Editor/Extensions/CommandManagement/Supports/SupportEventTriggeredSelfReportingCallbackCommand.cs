using System;
using Pharos.Extensions.EventManagement;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    internal class SupportEventTriggeredSelfReportingCallbackCommand
    {
        [Inject(true)]
        public IEvent UntypedEvent { get; private set; }

        [Inject(true)]
        public SupportEvent TypedEvent { get; private set; }

        [Inject("ExecuteCallback")]
        public Action<SupportEventTriggeredSelfReportingCallbackCommand> Callback { get; private set; }

        public void Execute()
        {
            Callback?.Invoke(this);
        }
    }
}