using System;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class ClassReportingCallbackGuard
    {
        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;

        public bool Approve()
        {
            reportingFunc?.Invoke(typeof(ClassReportingCallbackGuard));
            return true;
        }
    }
}