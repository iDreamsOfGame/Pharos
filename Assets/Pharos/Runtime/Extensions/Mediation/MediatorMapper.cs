using System;
using System.Collections.Generic;
using Pharos.Framework;

namespace Pharos.Extensions.Mediation
{
    public class MediatorMapper : IMediatorMapper, IMediatorUnmapper
    {
        private readonly Dictionary<Type, IMediatorMapping> mediatorTypeToMapping = new();

        private readonly Type viewType;

        private readonly IMediatorViewHandler viewHandler;

        private readonly ILogger logger;

        public MediatorMapper(Type viewType, IMediatorViewHandler viewHandler, ILogger logger)
        {
            this.viewType = viewType;
            this.viewHandler = viewHandler;
            this.logger = logger;
        }

        public IMediatorConfigurator ToMediator<T>() where T : IMediator
        {
            return ToMediator(typeof(T));
        }

        public IMediatorConfigurator ToMediator(Type mediatorType)
        {
            if (!mediatorType.IsAssignableFrom(typeof(IMediator)))
            {
                logger.LogError(new ArgumentException(nameof(mediatorType)));
                return null;
            }

            return mediatorTypeToMapping.TryGetValue(mediatorType, out var mapping) ? OverwriteMapping(mapping) : CreateMapping(mediatorType);
        }

        public void FromMediator<T>() where T : IMediator
        {
            FromMediator(typeof(T));
        }

        public void FromMediator(Type mediatorType)
        {
            if (!mediatorType.IsAssignableFrom(typeof(IMediator)))
            {
                logger.LogError(new ArgumentException(nameof(mediatorType)));
                return;
            }
            
            if (mediatorTypeToMapping.TryGetValue(mediatorType, out var mapping))
                DeleteMapping(mapping);
        }

        public void FromAll()
        {
            var mappings = mediatorTypeToMapping.Values;
            var copy = new IMediatorMapping[mappings.Count];
            mappings.CopyTo(copy, 0);
            foreach (var mapping in copy)
            {
                DeleteMapping(mapping);
            }
        }

        private MediatorMapping CreateMapping(Type mediatorType)
        {
            var mapping = new MediatorMapping(viewType, mediatorType);
            viewHandler.AddMapping(mapping);
            mediatorTypeToMapping[mediatorType] = mapping;
            logger?.LogDebug("{0} mapped to {1}", viewType, mapping);
            return mapping;
        }

        private void DeleteMapping(IMediatorMapping mapping)
        {
            viewHandler.RemoveMapping(mapping);
            mediatorTypeToMapping.Remove(mapping.MediatorType);
            logger?.LogDebug("0} unmapped from {1}", viewType, mapping);
        }

        private IMediatorConfigurator OverwriteMapping(IMediatorMapping mapping)
        {
            logger?.LogDebug("{0} already mapped to {1}\nIf you have overridden this mapping intentionally you can use 'unmap()' "
                             + "prior to your replacement mapping in order to avoid seeing this message.",
                viewType,
                mapping);
            DeleteMapping(mapping);
            return CreateMapping(mapping.MediatorType);
        }
    }
}