using System;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class SelfReportingCallbackCommand
    {
        [Inject("ExecuteCallback")]
        private Action<SelfReportingCallbackCommand> callback;
        
        public void Execute()
        {
            callback?.Invoke(this);
        }
    }
}