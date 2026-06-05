using System;
using System.Collections.Generic;
using VContainer;

namespace Pharos.Framework.Injection
{
    public interface IPharosInjector
    {
        IObjectResolver Container { get; }

        IScopedObjectResolver ScopedContainer { get; }

        ContainerBuilder Builder { get; }

        /// <summary>
        /// The parent IPharosInjector used for dependencies the current IPharosInjector can't supply. 
        /// </summary>
        IPharosInjector Parent { get; set; }

        List<IPharosInjector> Children { get; }

        /// <summary>
        /// Creates a new <see cref="IPharosInjector"/> and sets itself as that new <see cref="IPharosInjector"/>'s parentInjector. 
        /// </summary>
        /// <returns>The new <see cref="IPharosInjector"/> instance as child of this <see cref="IPharosInjector"/>. </returns>
        IPharosInjector CreateChild();

        /// <summary>
        /// Removes a child <see cref="IPharosInjector"/>.
        /// </summary>
        /// <param name="childInjector"></param>
        /// <returns></returns>
        bool RemoveChild(IPharosInjector childInjector);

        bool HasMapping<T>(object key = null);

        bool HasMapping(Type type, object key = null);

        InjectionMapping Map<T>(object key = null);

        InjectionMapping Map(Type type, object key = null);

        IPharosInjector BuildAncestors();

        IPharosInjector Build(bool buildAncestors = false, bool buildDescendants = false);

        IPharosInjector BuildDescendants();

        T GetInstance<T>(object key = null);

        object GetInstance(Type type, object key = null);

        T GetOrCreateNewInstance<T>(object key = null);

        object GetOrCreateNewInstance(Type type, object key = null);

        T CreateNewInstance<T>(object key = null);

        object CreateNewInstance(Type type, object key = null);

        void InjectInto(object target, IObjectResolver container = null);

        void Dispose();
    }
}