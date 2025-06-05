using System;
using Pharos.Common.CommandCenter;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class OptionalInjectionPointsCommand : ICommand
    {
        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;

        [Inject(true)]
        private string message;

        [Inject(true)]
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