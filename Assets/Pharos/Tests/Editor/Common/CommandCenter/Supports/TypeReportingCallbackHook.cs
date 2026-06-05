using System;
using Pharos.Framework;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class TypeReportingCallbackHook : IHook
    {
        [Inject, Key("ReportingFunction")]
        public Action<object> ReportingFunc { get; private set; }

        public void Hook()
        {
            ReportingFunc?.Invoke(typeof(TypeReportingCallbackHook));
        }
    }
}