using System;
using Pharos.Common.CommandCenter;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class CallbackCommand : ICommand
    {
        public const string CallbackKey = "ExecuteCallback";
        
        [Inject, Key(CallbackKey)]
        public Action Callback { get; set; }

        public void Execute()
        {
            Callback?.Invoke();
        }
    }
}