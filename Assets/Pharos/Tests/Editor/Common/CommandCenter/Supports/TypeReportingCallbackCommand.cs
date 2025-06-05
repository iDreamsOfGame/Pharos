using System;
using Pharos.Common.CommandCenter;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class TypeReportingCallbackCommand : ICommand
    {
        [Inject("ReportingFunction")]
        public Action<object> ReportingFunc { get; private set; }

        public void Execute()
        {
            ReportingFunc?.Invoke(typeof(TypeReportingCallbackCommand));
        }
    }
}