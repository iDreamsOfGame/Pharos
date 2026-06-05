using System;
using Pharos.Common.CommandCenter;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    [InjectIgnore]
    internal class TypeReportingCallbackCommand : ICommand
    {
        [Inject, Key("ReportingFunction")]
        public Action<object> ReportingFunc { get; private set; }

        public void Execute()
        {
            ReportingFunc?.Invoke(typeof(TypeReportingCallbackCommand));
        }
    }
}