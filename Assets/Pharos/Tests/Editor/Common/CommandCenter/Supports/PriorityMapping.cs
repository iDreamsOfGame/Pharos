using System;
using Pharos.Common.CommandCenter;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class PriorityMapping : CommandMapping
    {
        public PriorityMapping(Type commandType, int priority)
            : base(commandType)
        {
            Priority = priority;
        }

        public int Priority { get; }
    }
}