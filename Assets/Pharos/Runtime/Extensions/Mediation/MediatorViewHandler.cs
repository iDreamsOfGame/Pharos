using System;
using System.Collections.Generic;
using Pharos.Common.ViewCenter;

namespace Pharos.Extensions.Mediation
{
    public class MediatorViewHandler : IMediatorViewHandler
    {
        private readonly List<IMediatorMapping> mappings = new();
        
        private readonly Dictionary<Type, IMediatorMapping> viewTypeToMapping = new();

        private IMediatorManager manager;
        
        public MediatorViewHandler(IMediatorManager mediatorManager)
        {
            manager = mediatorManager;
        }

        public void AddMapping(IMediatorMapping mapping)
        {
            if (mappings.Contains(mapping))
                return;
            
            mappings.Add(mapping);
            FlushCache();
        }

        public void RemoveMapping(IMediatorMapping mapping)
        {
            var index = mappings.IndexOf(mapping);
            if (index == -1)
                return;
            
            mappings.RemoveAt(index);
            FlushCache();
        }
        
        public void HandleViewInitialized(IView view, Type viewType)
        {
            var mapping = GetMapping(viewType);
            manager.CreateMediator(view, viewType, mapping);
        }
        
        private void FlushCache()
        {
            viewTypeToMapping.Clear();
        }

        public IMediatorMapping GetMapping(Type viewType)
        {
            if (viewTypeToMapping.TryGetValue(viewType, out var value)) 
                return value;
           
            // Adds to dictionary to cache mapping for quick search.
            foreach (var mapping in mappings)
            {
                if (mapping.ViewType != viewType) 
                    continue;
                    
                viewTypeToMapping.Add(viewType, mapping);
                break;
            }

            return viewTypeToMapping[viewType];
        }
    }
}