using System.Collections.Generic;
using Pharos.Common.CommandCenter;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class PriorityMappingComparer : IComparer<ICommandMapping>
    {
        public int Compare(ICommandMapping a, ICommandMapping b)
        {
            var priorityA = a is PriorityMapping mapping ? mapping.Priority : 0;
            var priorityB = b is PriorityMapping priorityMapping ? priorityMapping.Priority : 0;
            return priorityA == priorityB ? 0 : priorityA > priorityB ? 1 : -1;
        }
    }
}