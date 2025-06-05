using System;
using Pharos.Framework;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class TypeReportingCallbackHook : IHook
    {
        [Inject("ReportingFunction")]
        public Action<object> ReportingFunc { get; private set; }

        public void Hook()
        {
            ReportingFunc?.Invoke(typeof(TypeReportingCallbackHook));
        }
    }
}