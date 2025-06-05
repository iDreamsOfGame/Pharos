using System;
using System.Collections.Generic;

namespace Pharos.Common.EventCenter
{
    public class EventDispatcher : IEventDispatcher
    {
        public static IEventDispatcher Instance { get; internal set; }

        private readonly Dictionary<Enum, List<EventMapConfig>> eventTypeToMapConfigs = new();

        public void AddEventListener<T>(Enum type, Action<T> listener)
        {
            AddEventListener(type, listener as Delegate);
        }

        public void AddEventListener(Enum type, Action<IEvent> listener)
        {
            AddEventListener(type, listener as Delegate);
        }

        public void AddEventListener(Enum type, Action listener)
        {
            AddEventListener(type, listener as Delegate);
        }

        public void AddEventListener(Enum type, Delegate listener)
        {
            if (!eventTypeToMapConfigs.ContainsKey(type))
                eventTypeToMapConfigs.Add(type, new List<EventMapConfig>());

            eventTypeToMapConfigs[type].Add(new EventMapConfig(listener));
        }

        public void RemoveEventListener<T>(Enum type, Action<T> listener)
        {
            RemoveEventListener(type, listener as Delegate);
        }

        public void RemoveEventListener(Enum type, Action<IEvent> listener)
        {
            RemoveEventListener(type, listener as Delegate);
        }

        public void RemoveEventListener(Enum type, Action listener)
        {
            RemoveEventListener(type, listener as Delegate);
        }

        public void RemoveEventListener(Enum type, Delegate listener)
        {
            if (!eventTypeToMapConfigs.TryGetValue(type, out var listeners))
                return;

            listeners.Remove(new EventMapConfig(listener));
            if (listeners.Count == 0)
                RemoveEventListeners(type);
        }

        public void RemoveEventListeners(Enum type)
        {
            eventTypeToMapConfigs.Remove(type);
        }

        public void RemoveEventListeners(object target)
        {
            var eventTypeToListeners = new Dictionary<Enum, List<Delegate>>();
            foreach (var (type, configs) in eventTypeToMapConfigs)
            {
                var listeners = new List<Delegate>();
                foreach (var config in configs)
                {
                    if (ReferenceEquals(config.Listener.Target, target) || config.Listener.Target == target)
                        listeners.Add(config.Listener);
                }

                if (listeners.Count > 0)
                    eventTypeToListeners.Add(type, listeners);
            }

            foreach (var (type, listeners) in eventTypeToListeners)
            {
                foreach (var listener in listeners)
                {
                    RemoveEventListener(type, listener);
                }
            }
        }

        public void RemoveAllEventListeners()
        {
            eventTypeToMapConfigs.Clear();
        }

        public bool HasEventListener(Enum type) => eventTypeToMapConfigs.ContainsKey(type);

        public void Dispatch(IEvent e)
        {
            if (!eventTypeToMapConfigs.TryGetValue(e.EventType, out var value))
                return;

            var listeners = value.ToArray();
            foreach (var listener in listeners)
            {
                listener.Invoke(e);
            }
        }
    }
}