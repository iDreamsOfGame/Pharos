using System;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class PayloadInjectionPointsCommand
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