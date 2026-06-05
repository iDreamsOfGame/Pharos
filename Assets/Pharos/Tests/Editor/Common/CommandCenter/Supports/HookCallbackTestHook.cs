using System;
using Pharos.Framework;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class HookCallbackTestHook : IHook
    {
        public const string CallbackFuncKey = "Callback";
        
        [Inject, Key(CallbackFuncKey)]
        private Action<object> reportingFunc;

        public void Hook()
        {
            reportingFunc?.Invoke(typeof(ClassReportingCallbackHook));
        }
    }
}