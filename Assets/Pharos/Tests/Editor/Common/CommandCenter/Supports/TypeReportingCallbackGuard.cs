using System;
using Pharos.Framework;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    [InjectIgnore]
    internal class TypeReportingCallbackGuard : IGuard
    {
        [Inject, Key("ReportingFunction")]
        private Action<object> reportingFunc;

        public bool Approve()
        {
            reportingFunc?.Invoke(typeof(TypeReportingCallbackGuard));
            return true;
        }
    }
}