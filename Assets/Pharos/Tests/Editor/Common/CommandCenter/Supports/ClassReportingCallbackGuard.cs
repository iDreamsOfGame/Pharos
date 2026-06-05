using System;
using Pharos.Framework;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class ClassReportingCallbackGuard : IGuard
    {
        [Inject, Key("ReportingFunction")]
        private Action<object> reportingFunc;

        public bool Approve()
        {
            reportingFunc?.Invoke(typeof(ClassReportingCallbackGuard));
            return true;
        }
    }
}