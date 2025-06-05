using System;
using Pharos.Common.EventCenter;

namespace PharosEditor.Tests.Common.EventCenter.Supports
{
    internal class BlankEvent : Event
    {
        public BlankEvent(Enum type)
            : base(type)
        {
        }
    }
}