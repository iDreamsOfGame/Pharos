using System;
using System.Collections.Generic;

namespace Pharos.Framework.Helpers
{
    internal class Pin : IPinEvent
    {
        private readonly Dictionary<object, bool> instanceToDetainedFlag = new();

        public event Action<object> Detained;

        public event Action<object> Released;

        public void Detain(object instance)
        {
            if (instance == null || !instanceToDetainedFlag.TryAdd(instance, true))
                return;

            Detained?.Invoke(instance);
        }

        public void Release(object instance)
        {
            if (instance == null || !instanceToDetainedFlag.Remove(instance))
                return;

            Released?.Invoke(instance);
        }

        public void ReleaseAll()
        {
            if (instanceToDetainedFlag == null || instanceToDetainedFlag.Count == 0)
                return;

            var instances = new object[instanceToDetainedFlag.Count];
            instanceToDetainedFlag.Keys.CopyTo(instances, 0);
            foreach (var instance in instances)
            {
                Release(instance);
            }
        }
    }
}