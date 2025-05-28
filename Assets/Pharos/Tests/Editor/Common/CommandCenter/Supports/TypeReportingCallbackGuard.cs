using System;
using Pharos.Framework;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class TypeReportingCallbackGuard : IGuard
    {
        [Inject("ReportingFunction")]
        private Action<object> reportingFunc;

        public bool Approve()
        {
            reportingFunc?.Invoke(typeof(TypeReportingCallbackGuard));
            return true;
        }
    }
}