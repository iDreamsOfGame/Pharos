using System;
using VContainer;

namespace Pharos.Framework.Injection
{
    public readonly struct InjectionMapping
    {
        public InjectionMapping(IPharosInjector injector,
            Type type,
            object key = null)
        {
            Injector = injector;
            Type = type;
            Key = key;
        }

        public IPharosInjector Injector { get; }

        public IObjectResolver Container => Injector.Container;

        public ContainerBuilder Builder => Injector.Builder;

        public Type Type { get; }

        public object Key { get; }

        public void AsSingleton()
        {
            Register(Lifetime.Singleton);
        }

        public void AsType()
        {
            Register(Lifetime.Transient);
        }

        public void AsScopedType()
        {
            Register(Lifetime.Scoped);
        }
        
        public void ToSingleton<T>()
        {
            ToSingleton(typeof(T));
        }

        public void ToSingleton(Type concrete)
        {
            Register(concrete, Lifetime.Singleton);
        }

        public void ToType<T>(bool autoBuild = false)
        {
            ToType(typeof(T), autoBuild);
        }

        public void ToType(Type concrete, bool autoBuild = false)
        {
            Register(concrete, Lifetime.Transient);
            
            if (autoBuild)
                Injector.Build(true, true);
        }
        
        public void ToScopedType<T>(bool autoBuild = false)
        {
            ToScopedType(typeof(T), autoBuild);
        }
        
        public void ToScopedType(Type concrete, bool autoBuild = false)
        {
            Register(concrete, Lifetime.Scoped);

            if (autoBuild)
                Injector.Build(true, true);
        }

        public void ToValue(object value, bool autoBuild = false, bool autoInject = false)
        {
            var registrationBuilder = Builder.RegisterInstance(value, Type);
            if (Key != null)
                registrationBuilder.Keyed(Key);

            if (autoBuild)
                Injector.Build(true, true);

            if (autoInject)
                Injector.InjectInto(value);
        }

        private void Register(Type concrete, Lifetime lifetime)
        {
            var registrationBuilder = Builder.Register(Type, concrete, lifetime);
            if (Key != null)
                registrationBuilder.Keyed(Key);
        }

        private void Register(Lifetime lifetime)
        {
            var registrationBuilder = Builder.Register(Type, lifetime);
            if (Key != null)
                registrationBuilder.Keyed(Key);
        }
    }
}