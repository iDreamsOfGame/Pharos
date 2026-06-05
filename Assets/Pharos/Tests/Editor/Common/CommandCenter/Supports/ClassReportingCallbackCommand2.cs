using System;
using Pharos.Common.CommandCenter;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class ClassReportingCallbackCommand2 : ICommand
    {
        [Inject, Key("ReportingFunction")]
        private Action<object> reportingFunc;

        public void Execute()
        {
            reportingFunc?.Invoke(typeof(ClassReportingCallbackCommand2));
        }
    }
}