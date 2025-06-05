using System;
using Pharos.Common.CommandCenter;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class CallbackCommand : ICommand
    {
        [Inject("ExecuteCallback")]
        public Action Callback { get; protected set; }

        public void Execute()
        {
            Callback?.Invoke();
        }
    }
}