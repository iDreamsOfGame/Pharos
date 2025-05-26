using System;

namespace Pharos.Extensions.EventManagement
{
    public interface IEvent
    {
        Enum EventType { get; }
    }
}