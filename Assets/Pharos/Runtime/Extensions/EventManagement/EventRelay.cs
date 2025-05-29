using System;
using System.Collections.Generic;

namespace Pharos.Extensions.EventManagement
{
    public class EventRelay
    {
        private readonly IEventDispatcher source;

        private readonly IEventDispatcher destination;

        private readonly List<Enum> types;

        private bool hasActivated;

        public EventRelay(IEventDispatcher source, IEventDispatcher destination, IEnumerable<Enum> types = null)
        {
            this.source = source;
            this.destination = destination;
            this.types = types == null ? new List<Enum>() : new List<Enum>(types);
        }

        public EventRelay Start()
        {
            if (!hasActivated)
            {
                hasActivated = true;
                AddListeners();
            }

            return this;
        }

        public EventRelay Stop()
        {
            if (hasActivated)
            {
                hasActivated = false;
                RemoveListeners();
            }

            return this;
        }

        public void AddType(Enum eventType)
        {
            types.Add(eventType);
            if (hasActivated)
                AddListener(eventType);
        }

        public void RemoveType(Enum eventType)
        {
            var index = types.IndexOf(eventType);
            if (index > -1)
            {
                types.RemoveAt(index);
                RemoveListener(eventType);
            }
        }

        private void AddListener(Enum type)
        {
            source.AddEventListener(type, destination.Dispatch);
        }

        private void AddListeners()
        {
            foreach (var type in types)
            {
                AddListener(type);
            }
        }

        private void RemoveListener(Enum type)
        {
            source.RemoveEventListener(type, destination.Dispatch);
        }

        private void RemoveListeners()
        {
            foreach (var type in types)
            {
                RemoveListener(type);
            }
        }
    }
}