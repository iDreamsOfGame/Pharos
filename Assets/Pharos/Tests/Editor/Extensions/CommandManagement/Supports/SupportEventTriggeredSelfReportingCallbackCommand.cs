using System;
using Pharos.Common.CommandCenter;
using Pharos.Common.EventCenter;
using VContainer;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    [InjectIgnore]
    internal class SupportEventTriggeredSelfReportingCallbackCommand : ICommand
    {
        [Inject]
        public IEvent UntypedEvent { get; private set; }

        [Inject]
        public SupportEvent TypedEvent { get; private set; }

        [Inject, Key("ExecuteCallback")]
        public Action<SupportEventTriggeredSelfReportingCallbackCommand> Callback { get; private set; }

        public void Execute()
        {
            Callback?.Invoke(this);
        }
    }
}