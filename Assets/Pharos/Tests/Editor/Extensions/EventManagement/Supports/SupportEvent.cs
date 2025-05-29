using System;
using Pharos.Extensions.EventManagement;

namespace PharosEditor.Tests.Extensions.EventManagement.Supports
{
    internal class SupportEvent : IEvent
    {
        public enum Type
        {
            Type1,

            Type2
        }

        public SupportEvent(Type type)
        {
            EventType = type;
        }

        public Enum EventType { get; }
    }
}