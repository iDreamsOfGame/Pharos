using System;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class TypeReportingCallbackGuard2
    {
        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;

        public bool Approve()
        {
            reportingFunc?.Invoke(typeof(TypeReportingCallbackGuard2));
            return true;
        }
    }
}