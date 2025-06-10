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

        public string Name => Builder?.Name;

        public Container Container { get; private set; }

        public ContainerBuilder Builder { get; }

        public IInjector Parent { get; set; }

        public List<IInjector> Children { get; private set; } = new();

        public IInjector GetChild(string name = null)
        {
            if (Children == null)
                return null;

            foreach (var child in Children)
            {
                if (child.Name == name)
                    return child;
            }

            return Children.Count > 0 ? Children[0] : null;
        }

        public IInjector CreateChild(string name = null)
        {
            var childName = !string.IsNullOrEmpty(name) ? name : $"Child-{Children.Count + 1}";
            if (!string.IsNullOrEmpty(Name))
                childName = $"{Name}/{childName}";
            
            var childInjector = new Injector(childName);
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

        public IInjector BuildAncestors()
        {
            if (Parent == null) 
                return this;
            
            var linkedList = new LinkedList<Injector>();
            var parent = Parent as Injector;
            while (parent != null)
            {
                linkedList.AddFirst(parent);
                parent = parent.Parent as Injector;
            }

            var ancestor = linkedList.First?.Value;
            while (ancestor != null)
            {
                linkedList.RemoveFirst();
                ancestor.Build();
                var container = ancestor.Container;

                if (linkedList.First != null)
                {
                    ancestor = linkedList.First.Value;
                    ancestor.Builder.SetParent(container);
                }
                else
                {
                    Builder.SetParent(container);
                    ancestor = null;
                }
            }

            return this;
        }

        public IInjector Build(bool buildAncestors = false, bool buildDescendants = false)
        {
            // Build Ancestors
            if (buildAncestors)
                BuildAncestors();

            Container = Builder.Build();

            if (buildDescendants)
                BuildDescendants();
            
            return this;
        }

        public IInjector BuildDescendants()
        {
            if (Children != null)
            {
                foreach (var child in Children)
                {
                    child.Builder.SetParent(Container);
                    child.Build(false, true);
                }
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
    }
}