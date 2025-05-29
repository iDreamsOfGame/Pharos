using System.Collections.Generic;
using Pharos.Framework.Helpers;
using Pharos.Framework.Injection;

namespace Pharos.Framework
{
    public partial class Context : IContext
    {
        private readonly LogManager logManager = new();

        private readonly List<IContext> children = new();

        private Pin pin;

        private LifecycleManager lifecycleManager;

        private ILogger logger;

        private ExtensionManager extensionManager;

        private ConfigManager configManager;

        public Context()
        {
            Setup();
        }

        public IInjector Injector { get; } = new Injector("Root");

        Pin IContext.Pin => pin;

        LifecycleManager IContext.LifecycleManager => lifecycleManager;

        LogManager IContext.LogManager => logManager;

        ExtensionManager IContext.ExtensionManager => extensionManager;

        ConfigManager IContext.ConfigManager => configManager;

        public IContext AddChild(IContext child)
        {
            if (child != null && !children.Contains(child))
            {
                logger.LogDebug("Adding child context {0}. ", child);
                if (child.HasInitialized)
                    logger.LogWarning("Child context {0} must be uninitialized. ", child);

                if (child.Injector.Parent != null)
                    logger.LogWarning("Child context {0} must not have a parent Injector. ", child);

                children.Add(child);
                child.Injector.Parent = Injector;
                child.Injector.Builder.SetParent(Injector.Container);
                child.Destroyed += OnChildDestroyed;
            }

            return this;
        }

        public IContext RemoveChild(IContext child)
        {
            if (child == null)
                return this;

            if (children.Contains(child))
            {
                logger.LogDebug("Removing child context {0}...", child);
                children.Remove(child);
                child.Injector.Parent = null;
                child.Destroyed -= OnChildDestroyed;
            }
            else
            {
                logger.LogWarning("Child context {0} must be a child of {1}", child, this);
            }

            return this;
        }

        private void Setup()
        {
            logger = logManager.GetLogger(this);
            pin = new Pin();
            lifecycleManager = new LifecycleManager(this);
            extensionManager = new ExtensionManager(this);
            configManager = new ConfigManager(this);

            Initializing += OnInitializing;
            Initialized += OnInitialized;
            Destroying += OnDestroying;
            Destroyed += OnDestroyed;

            Injector.Map<IContext>().ToValue(this);
            Injector.Map<IInjector>().ToValue(Injector);
        }

        private void RemoveChildren()
        {
            foreach (var child in children.ToArray())
            {
                RemoveChild(child);
            }

            children.Clear();
        }

        private void OnChildDestroyed(object context)
        {
            RemoveChild(context as IContext);
        }
    }
}