using System;
using System.Collections.Generic;
using Pharos.Framework;

namespace Pharos.Common.CommandCenter
{
    public class CommandMappingList : ICommandMappingList
    {
        private readonly Dictionary<Type, ICommandMapping> commandTypeMappingMap = new();
        
        private readonly List<ICommandMapping> mappings = new();
        
        private readonly ICommandTrigger trigger;

        private readonly IEnumerable<Action<ICommandMapping>> processors;

        private readonly ILogger logger;

        private IComparer<ICommandMapping> sortComparer;

        private bool hasSorted;
        
        public CommandMappingList(ICommandTrigger trigger, IEnumerable<Action<ICommandMapping>> processors, ILogger logger = null)
        {
            this.trigger = trigger;
            this.processors = processors;
            this.logger = logger;
        }

        public IEnumerable<ICommandMapping> Mappings
        {
            get
            {
                if (!hasSorted)
                    SortMappings();

                return mappings.ToArray();
            }
        }

        public ICommandMappingList WithSortComparer(IComparer<ICommandMapping> comparer)
        {
            sortComparer = comparer;
            hasSorted = false;
            return this;
        }

        public void AddMapping(ICommandMapping mapping)
        {
            hasSorted = false;
            InvokeProcessors(mapping);
            if (commandTypeMappingMap.TryGetValue(mapping.CommandType, out var oldMapping))
            {
                OverwriteMapping(oldMapping, mapping);
            }
            else
            {
                AddMappingToList(mapping);
                if (mappings.Count == 1)
                    trigger.Activate();
            }
        }

        public void RemoveMapping(ICommandMapping mapping)
        {
            if (!commandTypeMappingMap.ContainsKey(mapping.CommandType))
                return;
            
            RemoveMappingFromList(mapping);
            if (mappings.Count == 0)
                trigger.Deactivate();
        }

        public void RemoveMappingFor(Type commandType)
        {
            if (commandTypeMappingMap.TryGetValue(commandType, out var mapping))
                RemoveMapping(mapping);
        }

        public void RemoveAllMappings()
        {
            if (mappings.Count == 0)
                return;

            var mappingCount = mappings.Count;
            var tempMappings = mappings.GetRange(0, mappingCount);
            for (var i = 0; i < mappingCount; i++)
            {
                var mapping = tempMappings[i];
                RemoveMappingFromList(mapping);
            }
            trigger.Deactivate();
        }

        private void AddMappingToList(ICommandMapping mapping)
        {
            commandTypeMappingMap[mapping.CommandType] = mapping;
            mappings.Add(mapping);
            logger?.LogDebug("{0} mapped to {1}", trigger, mapping);
        }

        private void RemoveMappingFromList(ICommandMapping mapping)
        {
            commandTypeMappingMap.Remove(mapping.CommandType);
            mappings.Remove(mapping);
            logger?.LogDebug("{0} unmapped from {1}", trigger, mapping);
        }

        private void OverwriteMapping(ICommandMapping oldMapping, ICommandMapping newMapping)
        {
            logger?.LogWarning("{0} already mapped to {1}\n" +
                               "If you have overridden this mapping intentionally you can use 'unmap()' " +
                               "prior to your replacement mapping in order to avoid seeing this message.\n", trigger, oldMapping);
            RemoveMappingFromList(oldMapping);
            AddMappingToList(newMapping);
        }

        private void SortMappings()
        {
            if (sortComparer != null)
                mappings.Sort(sortComparer);

            hasSorted = true;
        }

        private void InvokeProcessors(ICommandMapping mapping)
        {
            if (processors == null)
                return;
            
            foreach (var processor in processors)
            {
                processor?.Invoke(mapping);
            }
        }
    }
}