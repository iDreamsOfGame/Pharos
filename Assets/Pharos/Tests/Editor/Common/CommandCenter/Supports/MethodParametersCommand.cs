using System;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class MethodParametersCommand
    {
        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;

        public void Execute(string message, int code)
        {
            if (reportingFunc != null)
            {
                reportingFunc.Invoke(message);
                reportingFunc.Invoke(code);
            }
        }
    }
}