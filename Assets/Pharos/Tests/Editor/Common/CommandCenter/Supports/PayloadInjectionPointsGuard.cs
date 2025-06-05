using System;
using Pharos.Framework;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class PayloadInjectionPointsGuard : IGuard
    {
        [Inject]
        private string message;

        [Inject]
        private int code;

        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;

        public bool Approve()
        {
            if (reportingFunc != null)
            {
                reportingFunc.Invoke(message);
                reportingFunc.Invoke(code);
            }

            return true;
        }
    }
}