using System;
using System.Collections.Generic;

namespace Pharos.Common.EventCenter
{
    public class EventDispatcher : IEventDispatcher
    {
        public static IEventDispatcher Instance { get; internal set; }

        private readonly Dictionary<Enum, List<EventListenerData>> eventTypeToListeners = new();

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
            if (!eventTypeToListeners.ContainsKey(type))
                eventTypeToListeners.Add(type, new List<EventListenerData>());

            eventTypeToListeners[type].Add(new EventListenerData(listener));
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
            if (!eventTypeToListeners.TryGetValue(type, out var listeners))
                return;

            listeners.Remove(new EventListenerData(listener));
            if (listeners.Count == 0)
                eventTypeToListeners.Remove(type);
        }

        public void RemoveAllEventListeners()
        {
            eventTypeToListeners.Clear();
        }

        public bool HasEventListener(Enum type) => eventTypeToListeners.ContainsKey(type);

        public void Dispatch(IEvent e)
        {
            if (!eventTypeToListeners.TryGetValue(e.EventType, out var value))
                return;

            var listeners = value.ToArray();
            foreach (var listener in listeners)
            {
                listener.Invoke(e);
            }
        }
    }
}