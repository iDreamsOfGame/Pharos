using System;
using System.Collections.Generic;

namespace Pharos.Framework.Helpers
{
    internal class Pin : IPinEvent
    {
        private readonly Dictionary<object, bool> instanceMap = new();

        public event Action<object> Detained;

        public event Action<object> Released;

        public void Detain(object instance)
        {
            if (instance == null || !instanceMap.TryAdd(instance, true))
                return;

            Detained?.Invoke(instance);
        }

        public void Release(object instance)
        {
            if (instance == null || !instanceMap.Remove(instance))
                return;

            Released?.Invoke(instance);
        }

        public void ReleaseAll()
        {
            if (instanceMap == null || instanceMap.Count == 0)
                return;

            var instances = new object[instanceMap.Keys.Count];
            instanceMap.Keys.CopyTo(instances, 0);
            foreach (var instance in instances)
            {
                Release(instance);
            }
        }
    }
}