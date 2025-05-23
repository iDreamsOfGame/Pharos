using System;
using System.Collections.Generic;
using System.Linq;
using Pharos.Framework.Injection;

namespace Pharos.Framework.Helpers
{
    internal class ConfigManager
    {
        private readonly ILogger logger;

        private bool hasInitialized;

        private Dictionary<object, bool> configMap = new();
        
        public ConfigManager(IContext context)
        {
            Context = context;
            logger = context.GetLogger(this);
            context.Initializing += OnContextInitializing;
        }

        public IContext Context { get; }
        
        public IInjector Injector => Context.Injector;

        public void AddConfig<T>() where T : IConfig
        {
            AddConfig(nameof(T));
        }

        public void AddConfig(object config)
        {
            if (!configMap.TryAdd(config, hasInitialized))
                return;

            if (!hasInitialized) 
                return;

            ProcessConfig(config);
        }

        public void Destroy()
        {
            configMap.Clear();
            if (Context != null)
                Context.Initializing -= OnContextInitializing;
        }
        
        private static void Configure(object obj, Type type = null)
        {
            if (obj is IConfig config)
            {
                config.Configure();
            }
            else
            {
                type ??= obj.GetType();
                const string methodName = nameof(IConfig.Configure);
                var method = type.GetMethod(methodName, Array.Empty<Type>());
                method?.Invoke(obj, null);
            }
        }
        
        private void OnContextInitializing(object obj)
        {
            if (!hasInitialized)
            {
                hasInitialized = true;
                foreach (var (config, hasProcessed) in configMap)
                {
                    if (hasProcessed)
                        continue;
                    
                    ProcessConfig(config);
                }

                configMap = configMap.ToDictionary(pair => pair.Key, _ => true);
            }
        }

        private void ProcessConfig(object config)
        {
            if (config is Type type)
            {
                logger.LogDebug("Now initializing. Instantiating config class {0}", config);
                ProcessConfigType(type);
            }
            else
            {
                logger.LogDebug("Now initializing. Injecting into config object {0}", config);
                ProcessConfigObject(config);
            }
        }

        private void ProcessConfigType(Type type)
        {
            var obj = Injector.GetOrCreateNewInstance(type);
            Configure(obj, type);
        }

        private void ProcessConfigObject(object obj)
        {
            Injector.InjectInto(obj);
            Configure(obj);
        }
    }
}