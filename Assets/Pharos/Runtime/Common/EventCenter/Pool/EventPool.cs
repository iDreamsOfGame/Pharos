using Pharos.Framework.Pool;

namespace Pharos.Common.EventCenter.Pool
{
    public sealed class EventPool<T> : LinkedObjectPool<T>
        where T : PooledEvent, IEvent, IPooledObject, new()
    {
    }
}