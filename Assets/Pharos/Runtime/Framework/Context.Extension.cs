using System;
using System.Collections.Generic;

namespace Pharos.Framework
{
    public partial class Context
    {
        public IContext AddExtension<T>() where T : IExtension
        {
            extensionManager?.Add<T>();
            return this;
        }

        public IContext AddExtension(Type type)
        {
            extensionManager?.Add(type);
            return this;
        }

        public IContext AddExtension(IExtension extension)
        {
            extensionManager?.Add(extension);
            return this;
        }

        public IContext AddExtensions(IEnumerable<Type> types)
        {
            extensionManager?.AddAll(types);
            return this;
        }
    }
}