using System;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class SelfReportingCallbackHook
    {
        [Inject]
        public SelfReportingCallbackCommand Command { get; private set; }

        [Inject("HookCallback")]
        private Action<SelfReportingCallbackHook> callback;

        public void Hook()
        {
            callback(this);
        }
    }
}