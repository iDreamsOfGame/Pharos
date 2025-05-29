using System;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class TypeReportingCallbackHook
    {
        [Inject("ReportingFunction")]
        public Action<object> ReportingFunc { get; private set; }

        public void Hook()
        {
            ReportingFunc?.Invoke(typeof(TypeReportingCallbackHook));
        }
    }
}