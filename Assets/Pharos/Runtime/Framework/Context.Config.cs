using System;
using System.Collections.Generic;

namespace Pharos.Framework
{
    public partial class Context
    {
        public IContext Configure<T>() where T : IConfig
        {
            configManager.AddConfig<T>();
            return this;
        }

        public IContext Configure(Type type)
        {
            configManager.AddConfig(type);
            return this;
        }

        public IContext Configure(params object[] objects)
        {
            if (objects == null)
                return this;

            foreach (var obj in objects)
            {
                configManager.AddConfig(obj);
            }

            return this;
        }

        public IContext ConfigureAll(IEnumerable<Type> types)
        {
            if (types == null)
                return this;

            foreach (var type in types)
            {
                Configure(type);
            }

            return this;
        }
    }
}