using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class MessageReturningCommand
    {
        [Inject]
        private string message;
        
        public string Execute() => message;
    }
}