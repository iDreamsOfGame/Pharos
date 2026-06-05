using System;
using Pharos.Common.CommandCenter;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    [InjectIgnore]
    internal class TypeReportingCallbackAsyncCommand2 : AsyncCommand
    {
        [Inject, Key("ReportingFunction")]
        private Action<object> reportingFunc;
        
        public override void Execute()
        {
            reportingFunc?.Invoke(typeof(TypeReportingCallbackAsyncCommand2));
            Executed();
        }
    }
}