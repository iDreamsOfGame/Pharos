using System;
using ReflexPlus;
using ReflexPlus.Core;

namespace Pharos.Framework.Injection
{
    public readonly struct InjectionMapping
    {
        public InjectionMapping(IInjector injector,
            Type type,
            object key = null)
        {
            Injector = injector;
            Type = type;
            Key = key;
        }

        public IInjector Injector { get; }

        public Container Container => Injector.Container;

        public ContainerBuilder Builder => Injector.Builder;

        public Type Type { get; }

        public object Key { get; }

        public void AsSingleton(bool initializeImmediately = false)
        {
            var resolution = initializeImmediately ? Resolution.Eager : Resolution.Lazy;
            Builder.RegisterType(Type, Key, Lifetime.Singleton, resolution);
        }

        public void ToType<T>(bool autoBuild = false)
        {
            ToType(typeof(T), autoBuild);
        }

        public void ToType(Type concrete, bool autoBuild = false)
        {
            Builder.RegisterType(concrete, Type, Key, Lifetime.Transient);
            
            if (autoBuild)
                Injector.Build(true, true);
        }

        public void ToValue(object value, bool autoBuild = false, bool autoInject = false)
        {
            Builder.RegisterValue(value, Type, Key);
            
            if (autoBuild)
                Injector.Build(true, true);
            
            if (autoInject) 
                Injector.InjectInto(value);
        }

        public void ToSingleton<T>(bool initializeImmediately = false)
        {
            ToSingleton(typeof(T), initializeImmediately);
        }

        public void ToSingleton(Type concrete, bool initializeImmediately = false)
        {
            var resolution = initializeImmediately ? Resolution.Eager : Resolution.Lazy;
            Builder.RegisterType(concrete, Type, Key, Lifetime.Singleton, resolution);
        }
    }
}