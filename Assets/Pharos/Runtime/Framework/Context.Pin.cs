using System;

namespace Pharos.Framework
{
    public partial class Context
    {
        public event Action<object> Detained
        {
            add => pin.Detained += value;
            remove => pin.Detained -= value;
        }

        public event Action<object> Released
        {
            add => pin.Released += value;
            remove => pin.Released -= value;
        }

        public IContext Detain(params object[] instances)
        {
            foreach (var instance in instances)
            {
                pin.Detain(instance);
            }

            return this;
        }

        public IContext Release(params object[] instances)
        {
            foreach (var instance in instances)
            {
                pin.Release(instance);
            }

            return this;
        }
    }
}