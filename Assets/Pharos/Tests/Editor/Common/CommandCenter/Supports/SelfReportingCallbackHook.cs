using System;
using Pharos.Framework;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    [InjectIgnore]
    internal class SelfReportingCallbackHook : IHook
    {
        [Inject]
        public SelfReportingCallbackCommand Command { get; private set; }

        [Inject, Key("HookCallback")]
        private Action<SelfReportingCallbackHook> callback;

        public void Hook()
        {
            callback(this);
        }
    }
}