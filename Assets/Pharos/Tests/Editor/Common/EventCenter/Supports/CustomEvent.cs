using Pharos.Common.EventCenter;

namespace PharosEditor.Tests.Common.EventCenter.Supports
{
    internal class CustomEvent : Event
    {
        public enum Type
        {
            A,

            B,

            C
        }

        public CustomEvent(Type type, string message)
            : base(type)
        {
            Message = message;
        }

        public string Message { get; }
    }
}