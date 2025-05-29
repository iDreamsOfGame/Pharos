using System;
using ReflexPlus.Attributes;

namespace Pharos.Common.CommandCenter
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