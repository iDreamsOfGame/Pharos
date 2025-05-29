using System;

namespace Pharos.Extensions.EventManagement
{
    [Serializable]
    public class Event : IEvent
    {
        public Event(Enum type)
        {
            EventType = type;
        }

        public Enum EventType { get; }
    }
}