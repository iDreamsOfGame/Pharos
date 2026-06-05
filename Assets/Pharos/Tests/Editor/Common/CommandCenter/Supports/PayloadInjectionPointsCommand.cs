using System;
using Pharos.Common.CommandCenter;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    [InjectIgnore]
    internal class PayloadInjectionPointsCommand : ICommand
    {
        [Inject]
        private string message;

        [Inject]
        private int code;

        [Inject, Key("ReportingFunction")]
        private Action<object> reportingFunc;

        public void Execute()
        {
            if (reportingFunc == null)
                return;

            reportingFunc.Invoke(message);
            reportingFunc.Invoke(code);
        }
    }
}