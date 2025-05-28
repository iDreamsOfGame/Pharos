using System;
using ReflexPlus.Attributes;

namespace Pharos.Common.CommandCenter
{
    internal class ClassReportingCallbackCommand2
    {
        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;
        
        public void Execute()
        {
            reportingFunc?.Invoke(typeof(ClassReportingCallbackCommand2));
        }
    }
}