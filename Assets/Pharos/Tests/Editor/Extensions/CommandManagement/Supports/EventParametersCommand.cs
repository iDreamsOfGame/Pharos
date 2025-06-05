using System;
using Pharos.Common.CommandCenter;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    internal class EventParametersCommand : ICommand
    {
        [Inject]
        private SupportEvent e;
        
        [Inject("ExecuteCallback")]
        private Action<SupportEvent> callback;

        public void Execute()
        {
            callback?.Invoke(e);
        }
    }
}