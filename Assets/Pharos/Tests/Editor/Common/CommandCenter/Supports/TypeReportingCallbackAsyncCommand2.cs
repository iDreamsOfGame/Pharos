using System;
using Pharos.Common.CommandCenter;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class TypeReportingCallbackAsyncCommand2 : AsyncCommand
    {
        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;
        
        public override void Execute()
        {
            reportingFunc?.Invoke(typeof(TypeReportingCallbackAsyncCommand2));
            Executed();
        }
    }
}