using System;
using System.Reflection;
using ReflexPlus.Core;
using ReflexPlus.Injectors;

namespace Pharos.Framework.Injection
{
    internal class Injector : IInjector
    {
        private const BindingFlags ConstructorsBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        
        public Injector(string name = null)
        {
            Builder = new ContainerBuilder()
                .SetName(name);
        }

        public Container Container { get; private set; }

        public ContainerBuilder Builder { get; }

        public IInjector Parent { get; set; }

        public IInjector CreateChild()
        {
            var childInjector = new Injector();
            childInjector.Parent = this;
            childInjector.Builder.SetParent(Container);
            return childInjector;
        }

        public bool HasMapping<T>(object key = null)
        {
            return HasMapping(typeof(T), key);
        }

        public bool HasMapping(Type type, object key = null)
        {
            var found = Container?.HasBinding(type, key) ?? false;
            if (!found)
                found = Builder.HasBinding(type, key);

            return found;
        }

        public InjectionMapping Map<T>(object key = null)
        {
            return Map(typeof(T), key);
        }

        public InjectionMapping Map(Type type, object key = null)
        {
            return new InjectionMapping(this, type, key);
        }

        public void Unmap<T>(object key = null)
        {
            Unmap(typeof(T), key);
        }

        public void Unmap(Type type, object key = null)
        {
            Container?.Unbind(type, key);
            Builder?.Unbind(type, key);
        }

        public IInjector Build()
        {
            if (Container != null)
                Builder.SetParent(Container);

            Container = Builder.Build();
            return this;
        }

        public T GetInstance<T>(object key = null)
        {
            return (T)GetInstance(typeof(T), key);
        }

        public object GetInstance(Type type, object key = null)
        {
            if (Container == null)
                Build();

            return Container?.Single(type, true, key);
        }

        public T GetOrCreateNewInstance<T>(object key = null)
        {
            return (T)GetOrCreateNewInstance(typeof(T), key);
        }

        public object GetOrCreateNewInstance(Type type, object key = null)
        {
            var instance = GetInstance(type, key);
            if (instance != null) 
                return instance;
            
            if (Container == null)
                Build();
            
            instance = Container?.Construct(type, key);
            return instance;
        }

        public void InjectInto(object target, Container container = null)
        {
            if (container == null)
            {
                container = Container;
                if (container == null)
                {
                    Build();
                    container = Container;
                }
            }

            AttributeInjector.InjectInto(target, container);
        }

        public void Dispose()
        {
            Container?.Dispose();
            Container = null;
        }
    }
}