using System;
using Pharos.Common.CommandCenter;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class SelfReportingCallbackCommand : ICommand
    {
        [Inject("ExecuteCallback")]
        private Action<SelfReportingCallbackCommand> callback;

        public void Execute()
        {
            callback?.Invoke(this);
        }
    }
}