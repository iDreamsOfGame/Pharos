using System;

namespace Pharos.Framework
{
    public partial class Context
    {
        public event Action<Exception> ErrorOccurred
        {
            add
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.ErrorOccurred += value;
            }
            remove
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.ErrorOccurred -= value;
            }
        }

        public event Action StateChanged
        {
            add
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.StateChanged += value;
            }
            remove
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.StateChanged -= value;
            }
        }

        public event Action<object> Initializing
        {
            add
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Initializing += value;
            }
            remove
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Initializing -= value;
            }
        }

        public event Action<object> Initialized
        {
            add
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Initialized += value;
            }
            remove
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Initialized -= value;
            }
        }

        public event Action<object> Suspending
        {
            add
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Suspending += value;
            }
            remove
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Suspending -= value;
            }
        }

        public event Action<object> Suspended
        {
            add
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Suspended += value;
            }
            remove
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Suspended -= value;
            }
        }

        public event Action<object> Resuming
        {
            add
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Resuming += value;
            }
            remove
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Resuming -= value;
            }
        }

        public event Action<object> Resumed
        {
            add
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Resumed += value;
            }
            remove
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Resumed -= value;
            }
        }

        public event Action<object> Destroying
        {
            add
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Destroying += value;
            }
            remove
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Destroying -= value;
            }
        }

        public event Action<object> Destroyed
        {
            add
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Destroyed += value;
            }
            remove
            {
                if (lifecycleManager == null)
                    return;

                lifecycleManager.Destroyed -= value;
            }
        }

        public LifecycleState State => lifecycleManager?.State ?? LifecycleState.UnInitialized;

        public bool HasInitialized => lifecycleManager?.HasInitialized ?? false;

        public bool HasActivated => lifecycleManager?.HasActivated ?? false;

        public bool HasSuspended => lifecycleManager?.HasSuspended ?? false;

        public bool HasDestroyed => lifecycleManager?.HasDestroyed ?? false;

        public void Initialize(Action<Exception> callback = null)
        {
            Injector.Build(false, true);
            lifecycleManager?.Initialize(callback);
        }

        public void Suspend(Action<Exception> callback = null)
        {
            lifecycleManager?.Suspend(callback);
        }

        public void Resume(Action<Exception> callback = null)
        {
            lifecycleManager?.Resume(callback);
        }

        public void Destroy(Action<Exception> callback = null)
        {
            lifecycleManager?.Destroy(callback);
        }

        private void OnInitializing(object context)
        {
            logger.LogDebug("Initializing...");
        }

        private void OnInitialized(object context)
        {
            logger.LogDebug("Initialized");
        }

        private void OnDestroying(object context)
        {
            logger.LogDebug("Destroying...");
        }

        private void OnDestroyed(object context)
        {
            Initializing -= OnInitializing;
            Initialized -= OnInitialized;
            Destroying -= OnDestroying;
            Destroyed -= OnDestroyed;

            extensionManager?.Destroy();
            configManager?.Destroy();
            pin?.ReleaseAll();
            Injector?.Dispose();
            RemoveChildren();
            logger.LogDebug("Destroyed!");
            logManager.Destroy();
        }
    }
}