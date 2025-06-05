using System;
using Pharos.Framework;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class SelfReportingCallbackHook : IHook
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