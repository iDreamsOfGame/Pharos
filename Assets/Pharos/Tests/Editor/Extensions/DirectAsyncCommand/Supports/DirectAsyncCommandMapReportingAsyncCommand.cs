using System;
using Pharos.Common.CommandCenter;
using Pharos.Extensions.DirectAsyncCommand;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Extensions.DirectAsyncCommand.Supports
{
    internal class DirectAsyncCommandMapReportingAsyncCommand : AsyncCommand
    {
        [Inject]
        private IDirectAsyncCommandMap dcm;

        [Inject("ReportingFunction")]
        private Action<IDirectAsyncCommandMap> reportingFunc;
        
        public override void Execute()
        {
            reportingFunc.Invoke(dcm);
            Executed();
        }
    }
}