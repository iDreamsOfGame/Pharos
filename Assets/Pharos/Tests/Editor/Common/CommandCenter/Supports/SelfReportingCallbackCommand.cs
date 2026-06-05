using System;
using Pharos.Common.CommandCenter;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    [InjectIgnore]
    internal class SelfReportingCallbackCommand : ICommand
    {
        public const string CallbackKey = "ExecuteCallback";
        
        [Inject, Key(CallbackKey)]
        private Action<SelfReportingCallbackCommand> callback;

        public void Execute()
        {
            callback?.Invoke(this);
        }
    }
}