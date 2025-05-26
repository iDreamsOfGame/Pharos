using System;
using System.Collections.Generic;

namespace Pharos.Extensions.EventManagement
{
    public class EventDispatcher : IEventDispatcher
    {
        public static IEventDispatcher GlobalEventDispatcher { get; internal set; }

        private readonly Dictionary<Enum, List<EventListenerData>> eventTypeListenerMap = new();
        
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
            if (!eventTypeListenerMap.ContainsKey(type))
                eventTypeListenerMap.Add(type, new List<EventListenerData>());
            
            eventTypeListenerMap[type].Add(new EventListenerData(listener));
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
            if (!eventTypeListenerMap.TryGetValue(type, out var listeners))
                return;

            listeners.Remove(new EventListenerData(listener));
            if (listeners.Count == 0)
                eventTypeListenerMap.Remove(type);
        }

        public void RemoveAllEventListeners()
        {
            eventTypeListenerMap.Clear();
        }

        public bool HasEventListener(Enum type) => eventTypeListenerMap.ContainsKey(type);

        public void Dispatch(IEvent e)
        {
            if (!eventTypeListenerMap.TryGetValue(e.EventType, out var value))
                return;
            
            var listeners = value.ToArray();
            foreach (var listener in listeners)
            {
                listener.Invoke(e);
            }
        }
    }
}