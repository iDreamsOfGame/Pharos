using System;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class CallbackCommand
    {
        [Inject("ExecuteCallback")]
        public Action Callback { get; protected set; }

        public void Execute()
        {
            Callback?.Invoke();
        }
    }
}