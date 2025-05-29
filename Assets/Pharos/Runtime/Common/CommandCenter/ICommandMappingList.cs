using System;
using System.Collections.Generic;

namespace Pharos.Common.CommandCenter
{
    public interface ICommandMappingList
    {
        IEnumerable<ICommandMapping> Mappings { get; }

        ICommandMappingList WithSortComparer(IComparer<ICommandMapping> comparer);

        void AddMapping(ICommandMapping mapping);

        void RemoveMapping(ICommandMapping mapping);

        void RemoveMappingFor(Type commandType);

        void RemoveAllMappings();
    }
}