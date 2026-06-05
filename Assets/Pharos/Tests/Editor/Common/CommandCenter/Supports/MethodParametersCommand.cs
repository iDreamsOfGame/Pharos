using System;
using Pharos.Common.CommandCenter;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class MethodParametersCommand : ICommand
    {
        [Inject]
        private string message;

        [Inject]
        private int code;
        
        [Inject, Key("ReportingFunction")]
        private Action<object> reportingFunc;

        public void Execute()
        {
            reportingFunc?.Invoke(message);
            reportingFunc?.Invoke(code);
        }
    }
}