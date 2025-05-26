using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pharos.Framework.Helpers
{
    internal class ExtensionManager
    {
        private readonly Dictionary<Type, object> typeMap = new();

        private readonly ILogger logger;

        public ExtensionManager(IContext context)
        {
            Context = context;
            logger = context.GetLogger(this);
        }

        public IContext Context { get; }

        public void Add<T>() where T : IExtension
        {
            Add(typeof(T));
        }

        public void Add(Type type)
        {
            var obj = CreateInstance(type);
            Add(obj);
        }

        public void Add(object obj)
        {
            if (obj == null)
                return;

            var type = obj.GetType();
            if (typeMap.ContainsKey(type))
                return;

            logger.LogDebug("Adding extension {0}...", obj);
            typeMap[type] = obj;

            if (obj is IExtension extension)
            {
                extension.Enable(Context);
            }
            else
            {
                const string methodName = nameof(IExtension.Enable);
                var method = type.GetMethod(methodName);
                method?.Invoke(obj, new object[] { Context });
            }
        }

        public void AddAll(IEnumerable<Type> types)
        {
            if (types == null)
                return;

            foreach (var type in types)
            {
                Add(type);
            }
        }

        public void RemoveAll()
        {
            foreach (var (type, obj) in typeMap)
            {
                Uninstall(type, obj);
            }
        }

        public void Destroy()
        {
            RemoveAll();
            typeMap?.Clear();
        }

        private static object CreateInstance(Type type)
        {
            // Get the constructor with the least amount of parameters. 
            var maxParameters = int.MaxValue;
            var constructors = type.GetConstructors();
            ConstructorInfo constructorToInject = null;
            foreach (var constructor in constructors)
            {
                var paramsLength = constructor.GetParameters().Length;
                if (paramsLength < maxParameters)
                {
                    constructorToInject = constructor;
                    maxParameters = paramsLength;
                }
            }

            if (constructorToInject != null)
            {
                var parameters = constructorToInject.GetParameters();
                var parametersLength = parameters.Length;
                var args = new object[parametersLength];
                for (var i = 0; i < parametersLength; i++)
                {
                    args[i] = parameters[i].DefaultValue;
                }

                return constructorToInject.Invoke(args);
            }

            return null;
        }

        private void Uninstall(Type type, object obj)
        {
            if (obj is IExtension extension)
            {
                extension.Disable(Context);
            }
            else
            {
                const string methodName = nameof(IExtension.Disable);
                var method = type.GetMethod(methodName);
                method?.Invoke(obj, new object[] { Context });
            }
        }
    }
}