using System;
using Pharos.Framework.Injection;

namespace Pharos.Framework.Helpers
{
    internal static class Actors
    {
        public static T CreateInstance<T>(IInjector injector = null) => (T)CreateInstance(typeof(T), injector);
        
        public static object CreateInstance(Type type, IInjector injector = null) => injector != null ? injector.CreateNewInstance(type) : Activator.CreateInstance(type);
    }
}