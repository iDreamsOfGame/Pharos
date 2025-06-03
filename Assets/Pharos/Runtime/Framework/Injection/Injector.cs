using System;
using System.Collections.Generic;
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

        public List<IInjector> Children { get; private set; } = new();

        public IInjector CreateChild()
        {
            var childInjector = new Injector();
            childInjector.Parent = this;
            childInjector.Builder.SetParent(Container);
            Children.Add(childInjector);
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

        public IInjector Build(bool buildAncestors = false)
        {
            // Build Ancestors
            if (buildAncestors)
                TryBuildAncestors();

            Container = Builder.Build();

            foreach (var child in Children)
            {
                child.Builder.SetParent(Container);
            }
            
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
            return instance ?? CreateNewInstance(type, key);
        }

        public T CreateNewInstance<T>(object key = null)
        {
            return (T)CreateNewInstance(typeof(T), key);
        }

        public object CreateNewInstance(Type type, object key = null)
        {
            if (Container == null)
                Build();

            return Container?.Construct(type, key);
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

            if (Children != null)
            {
                foreach (var child in Children)
                {
                    child?.Dispose();
                }

                Children = null;
            }
        }

        private void TryBuildAncestors()
        {
            if (Parent == null) 
                return;
            
            var linkedList = new LinkedList<Injector>();
            var parent = Parent as Injector;
            while (parent != null)
            {
                linkedList.AddFirst(parent);
                parent = parent.Parent as Injector;
            }

            var first = linkedList.First.Value;
            while (first != null)
            {
                linkedList.RemoveFirst();
                first.Build();
                var container = first.Container;

                if (linkedList.First != null)
                {
                    first = linkedList.First.Value;
                    first.Builder.SetParent(container);
                }
                else
                {
                    first = null;
                }
            }
        }
    }
}