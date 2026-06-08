using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using VContainer;

namespace Pharos.Framework.Injection
{
    internal class Injector : IPharosInjector
    {
        private const BindingFlags ConstructorsBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        
        [Inject]
        public Injector(ContainerBuilder containerBuilder = null)
        {
            Builder = containerBuilder ?? new ContainerBuilder();
        }

        public IObjectResolver Container { get; private set; }

        public IScopedObjectResolver ScopedContainer
        {
            get
            {
                return Container switch
                {
                    null => null,
                    ScopedContainer scopedContainer => scopedContainer,
                    _ => (Container as Container)!.RootScope
                };
            }
        }

        public ContainerBuilder Builder { get; }

        public IPharosInjector Parent { get; set; }

        public List<IPharosInjector> Children { get; private set; } = new();

        public IPharosInjector CreateChild()
        {
            if (Container == null)
                Build();

            IObjectResolver root;
            IScopedObjectResolver parent;
            if (Container is ScopedContainer scopedContainer)
            {
                root = scopedContainer.Root;
                parent = scopedContainer;
            }
            else
            {
                root = Container;
                parent = (Container as Container)!.RootScope;
            }
            
            var childContainerBuilder = new ScopedContainerBuilder(root, parent)
            {
                ApplicationOrigin = Container!.ApplicationOrigin
            };
            var childInjector = new Injector(childContainerBuilder)
            {
                Parent = this
            };
            Children.Add(childInjector);
            return childInjector;
        }

        public bool RemoveChild(IPharosInjector childInjector)
        {
            if (childInjector == null)
                return false;
            
            if (!ReferenceEquals(childInjector.Parent, this))
                return false;
            
            childInjector.Parent = null;
            childInjector.Dispose();
            return Children?.Remove(childInjector) ?? false;
        }

        public bool HasMapping<T>(object key = null)
        {
            return HasMapping(typeof(T), key);
        }

        public bool HasMapping(Type type, object key = null)
        {
            var container = ScopedContainer;
            while (container != null)
            {
                if (container.TryGetRegistration(type, out _, key))
                    return true;
                
                container = container.Parent;
            }

            return false;
        }

        public InjectionMapping Map<T>(object key = null)
        {
            return Map(typeof(T), key);
        }

        public InjectionMapping Map(Type type, object key = null)
        {
            return new InjectionMapping(this, type, key);
        }

        public IPharosInjector BuildAncestors()
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
                ancestor = linkedList.First?.Value;
            }

            return this;
        }

        public IPharosInjector Build(bool buildAncestors = false, bool buildDescendants = false)
        {
            if (buildAncestors)
                BuildAncestors();

            var sharedInstances = Container != null 
                ? new ConcurrentDictionary<Registration, object>(Container.SharedInstances)
                : new ConcurrentDictionary<Registration, object>();
            Container?.Dispose();
            Container = Builder.Build(sharedInstances);

            if (Container != null)
            {
                Container.ThrowIfUnresolved = false;

                // Updates parent reference.
                if (Parent != null && ScopedContainer != null)
                    ScopedContainer.Parent = Parent.ScopedContainer;
            }

            if (buildDescendants)
                BuildDescendants();
            
            // Updates children containers references
            if (Children != null)
            {
                foreach (var child in Children)
                {
                    if (child.Container == null)
                        continue;

                    child.ScopedContainer.Parent = ScopedContainer;
                }
            }
            
            return this;
        }

        public IPharosInjector BuildDescendants()
        {
            if (Children != null)
            {
                foreach (var child in Children)
                {
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
                Build(true);
            
            if (Container == null)
                return null;

            Container.TryResolve(type, out var result, key);
            return result;
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
            if (!HasMapping(type, key))
                Builder.Register(type, Lifetime.Transient);
            
            Build();

            if (Container == null)
                return null;

            Container.TryResolve(type, out var result, key);
            return result;
        }

        public void InjectInto(object target, IObjectResolver container = null)
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

            container?.Inject(target);
        }

        public void Dispose()
        {
            if (Children != null)
            {
                foreach (var child in Children)
                {
                    child?.Dispose();
                }

                Children = null;
            }

            Container?.Dispose();
            Container = null;
        }
    }
}