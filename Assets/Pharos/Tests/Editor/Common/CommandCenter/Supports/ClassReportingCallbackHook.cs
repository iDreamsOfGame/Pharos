using System;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class ClassReportingCallbackHook
    {
        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;

        public void Hook()
        {
            reportingFunc?.Invoke(typeof(ClassReportingCallbackHook));
        }
    }
}