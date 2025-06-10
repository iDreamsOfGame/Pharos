using System;
using System.Collections.Generic;
using ReflexPlus.Core;

namespace Pharos.Framework.Injection
{
    public interface IInjector : IDisposable
    {
        string Name { get; }

        Container Container { get; }

        ContainerBuilder Builder { get; }

        /// <summary>
        /// The parent IInjector used for dependencies the current IInjector can't supply. 
        /// </summary>
        IInjector Parent { get; set; }

        List<IInjector> Children { get; }

        IInjector GetChild(string name = null);

        /// <summary>
        /// Creates a new <see cref="IInjector"/> and sets itself as that new <see cref="IInjector"/>'s parentInjector. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The new <see cref="IInjector"/> instance as child of this <see cref="IInjector"/>. </returns>
        IInjector CreateChild(string name = null);

        bool HasMapping<T>(object key = null);

        bool HasMapping(Type type, object key = null);

        InjectionMapping Map<T>(object key = null);

        InjectionMapping Map(Type type, object key = null);

        void Unmap<T>(object key = null);

        void Unmap(Type type, object key = null);

        IInjector Build(bool buildAncestors = false, bool buildDescendants = false);

        T GetInstance<T>(object key = null);

        object GetInstance(Type type, object key = null);

        T GetOrCreateNewInstance<T>(object key = null);

        object GetOrCreateNewInstance(Type type, object key = null);

        T CreateNewInstance<T>(object key = null);

        object CreateNewInstance(Type type, object key = null);

        void InjectInto(object target, Container container = null);
    }
}