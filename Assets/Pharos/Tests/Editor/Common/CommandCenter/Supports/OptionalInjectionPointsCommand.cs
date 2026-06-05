using System;
using Pharos.Common.CommandCenter;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    [InjectIgnore]
    internal class OptionalInjectionPointsCommand : ICommand
    {
        [Inject, Key("ReportingFunction")]
        private Action<object> reportingFunc;

        [Inject]
        private string message;

        [Inject]
        private int code;

        public void Execute()
        {
            if (reportingFunc != null)
            {
                reportingFunc.Invoke(message);
                reportingFunc.Invoke(code);
            }
        }
    }
}