using System;
using Pharos.Framework.Injection;

namespace Pharos.Framework.Helpers
{
    internal static class InstanceActivator
    {
        public static T CreateInstance<T>(IPharosInjector injector = null) => (T)CreateInstance(typeof(T), injector);

        public static object CreateInstance(Type type, IPharosInjector injector = null)
        {
            return injector != null ? injector.CreateNewInstance(type) : Activator.CreateInstance(type);
        }
    }
}