using System;
using Pharos.Common.CommandCenter;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    [InjectIgnore]
    internal class TypeReportingCallbackCommand2 : ICommand
    {
        [Inject, Key("ReportingFunction")]
        private Action<object> reportingFunc;

        public void Execute()
        {
            reportingFunc?.Invoke(typeof(TypeReportingCallbackCommand2));
        }
    }
}