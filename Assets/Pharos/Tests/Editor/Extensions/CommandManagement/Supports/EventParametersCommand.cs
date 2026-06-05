using System;
using Pharos.Common.CommandCenter;
using VContainer;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    [InjectIgnore]
    internal class EventParametersCommand : ICommand
    {
        [Inject]
        private SupportEvent e;
        
        [Inject, Key("ExecuteCallback")]
        private Action<SupportEvent> callback;

        public void Execute()
        {
            callback?.Invoke(e);
        }
    }
}