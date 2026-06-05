using System;
using Pharos.Common.CommandCenter;
using Pharos.Extensions.DirectAsyncCommand;
using VContainer;

namespace PharosEditor.Tests.Extensions.DirectAsyncCommand.Supports
{
    internal class DirectAsyncCommandMapReportingAsyncCommand : AsyncCommand
    {
        [Inject]
        private IDirectAsyncCommandMap directAsyncCommandMap;

        [Inject, Key("ReportingFunction")]
        private Action<IDirectAsyncCommandMap> reportingFunc;
        
        public override void Execute()
        {
            reportingFunc.Invoke(directAsyncCommandMap);
            Executed();
        }
    }
}