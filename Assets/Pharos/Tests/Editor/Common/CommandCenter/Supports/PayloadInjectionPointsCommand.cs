using System;
using Pharos.Common.CommandCenter;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class PayloadInjectionPointsCommand : ICommand
    {
        [Inject]
        private string message;

        [Inject]
        private int code;

        [Inject("ReportingFunction")]
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