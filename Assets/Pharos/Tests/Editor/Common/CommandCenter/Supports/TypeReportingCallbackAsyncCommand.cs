using System;
using Pharos.Common.CommandCenter;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class TypeReportingCallbackAsyncCommand : AsyncCommand
    {
        [Inject("ReportingFunction")]
        public Action<object> ReportingFunc { get; private set; }
        
        public override void Execute()
        {
            ReportingFunc?.Invoke(typeof(TypeReportingCallbackAsyncCommand));
            Executed();
        }
    }
}