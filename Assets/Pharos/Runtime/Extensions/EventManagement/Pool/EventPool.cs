using Pharos.Framework.Pool;

namespace Pharos.Extensions.EventManagement.Pool
{
    public sealed class EventPool<T> : LinkedObjectPool<T>
        where T : PooledEvent, IEvent, IPooledObject, new()
    {
    }
}