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

        public void ToType<T>()
        {
            ToType(typeof(T));
        }

        public void ToType(Type concrete)
        {
            Builder.RegisterType(concrete, Type, Key, Lifetime.Transient);
        }

        public void ToValue(object value, bool autoInject = false)
        {
            if (autoInject)
            {
                var newBuilder = new ContainerBuilder();
                if (Container != null)
                    newBuilder.SetParent(Container);

                var newContainer = newBuilder.RegisterValue(value, Type, Key)
                    .Build();

                Injector.InjectInto(value, newContainer);
            }
            else
            {
                Builder.RegisterValue(value, Type, Key);
            }
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