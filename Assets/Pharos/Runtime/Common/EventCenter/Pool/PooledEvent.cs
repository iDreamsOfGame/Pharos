using System;
using Pharos.Framework.Pool;

namespace Pharos.Common.EventCenter.Pool
{
    public abstract class PooledEvent : IEvent, IPooledObject
    {
        public Enum EventType { get; set; }

        public abstract void OnPreprocessGet();

        public abstract void OnPreprocessReturn();
    }

    public abstract class PooledEvent<T> : PooledEvent
        where T : PooledEvent, new()
    {
        public static EventPool<T> ObjectPool { get; } = new();

        public static void Borrow(Action<T> callback)
        {
            var eventArgs = ObjectPool.Get();
            if (eventArgs == null)
                return;

            callback?.Invoke(eventArgs);
            ObjectPool.Return(eventArgs);
        }

        public static void Dispatch(Enum eventType, Action<T> setter = null)
        {
            Dispatch(eventType, setter, EventDispatcher.Instance);
        }

        public static void Dispatch(Enum eventType, Action<T> setter, IEventDispatcher eventDispatcher)
        {
            Borrow(e =>
            {
                e.EventType = eventType;
                setter?.Invoke(e);
                eventDispatcher?.Dispatch(e);
            });
        }
    }
}