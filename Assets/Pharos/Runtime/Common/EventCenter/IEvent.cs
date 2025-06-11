using System;

namespace Pharos.Common.EventCenter
{
    public interface IEvent
    {
        Enum EventType { get; set; }
    }
}