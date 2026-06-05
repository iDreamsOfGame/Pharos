using System;
using Pharos.Framework;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    [InjectIgnore]
    internal class ClassReportingCallbackHook : IHook
    {
        [Inject, Key("ReportingFunction")]
        private Action<object> reportingFunc;

        public void Hook()
        {
            reportingFunc?.Invoke(typeof(ClassReportingCallbackHook));
        }
    }
}