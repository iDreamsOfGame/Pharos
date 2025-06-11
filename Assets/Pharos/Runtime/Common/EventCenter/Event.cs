using System;

namespace Pharos.Common.EventCenter
{
    [Serializable]
    public class Event : IEvent
    {
        public Event(Enum type)
        {
            EventType = type;
        }

        public Enum EventType { get; set; }
    }
}