using System;
using Pharos.Framework;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class PayloadInjectionPointsHook : IHook
    {
        [Inject]
        private string message;

        [Inject]
        private int code;

        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;

        public void Hook()
        {
            if (reportingFunc == null)
                return;

            reportingFunc.Invoke(message);
            reportingFunc.Invoke(code);
        }
    }
}