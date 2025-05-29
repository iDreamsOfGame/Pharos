using System;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class TypeReportingCallbackCommand
    {
        [Inject("ReportingFunction")]
        public Action<object> ReportingFunc { get; private set; }

        public void Execute()
        {
            ReportingFunc?.Invoke(typeof(TypeReportingCallbackCommand));
        }
    }
}