using System;
using Pharos.Common.CommandCenter;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class ClassReportingCallbackCommand : ICommand
    {
        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;

        public void Execute()
        {
            reportingFunc?.Invoke(typeof(ClassReportingCallbackCommand));
        }
    }
}