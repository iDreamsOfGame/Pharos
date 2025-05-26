using System;
using Pharos.Extensions.EventManagement;

namespace PharosEditor.Tests.Extensions.EventManagement.Supports
{
    internal class BlankEvent : Event
    {
        public BlankEvent(Enum type)
            : base(type)
        {
        }
    }
}