using System;
using ReflexPlus.Attributes;

namespace Pharos.Common.CommandCenter
{
    internal class ClassReportingCallbackCommand
    {
        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;
        
        public void Execute()
        {
            reportingFunc?.Invoke(typeof(ClassReportingCallbackCommand));
        }
    }
}