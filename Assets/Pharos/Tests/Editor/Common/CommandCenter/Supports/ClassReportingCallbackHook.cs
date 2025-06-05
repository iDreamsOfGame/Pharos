using System;
using Pharos.Framework;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class ClassReportingCallbackHook : IHook
    {
        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;

        public void Hook()
        {
            reportingFunc?.Invoke(typeof(ClassReportingCallbackHook));
        }
    }
}