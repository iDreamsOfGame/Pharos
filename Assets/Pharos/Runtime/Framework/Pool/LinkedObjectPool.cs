using System.Collections.Generic;

namespace Pharos.Framework.Pool
{
    public class LinkedObjectPool<T>
        where T : class, IPooledObject, new()
    {
        private readonly LinkedList<T> pool = new();

        public virtual T Get()
        {
            if (pool == null)
                return null;

            if (pool.Count == 0)
                return new T();

            var item = pool.First?.Value;
            pool.RemoveFirst();
            item?.OnPreprocessGet();
            return item;
        }

        public virtual void Return(T e)
        {
            e.OnPreprocessReturn();
            pool.AddLast(e);
        }

        public virtual void Clear()
        {
            pool.Clear();
        }
    }
}