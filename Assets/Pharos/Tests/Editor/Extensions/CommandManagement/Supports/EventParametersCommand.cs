using System;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    internal class EventParametersCommand
    {
        [Inject("ExecuteCallback")]
        private Action<SupportEvent> callback;
        
        public void Execute(SupportEvent e)
        {
            callback?.Invoke(e);
        }
    }
}