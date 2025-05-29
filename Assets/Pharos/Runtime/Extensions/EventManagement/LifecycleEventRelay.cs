using Pharos.Framework;

namespace Pharos.Extensions.EventManagement
{
    internal class LifecycleEventRelay
    {
        private ILifecycleEvent source;

        public LifecycleEventRelay(ILifecycleEvent source)
        {
            this.source = source;

            source.StateChanged += OnStateChanged;

            source.Initializing += OnInitializing;
            source.Initialized += OnInitialized;

            source.Suspending += OnSuspending;
            source.Suspended += OnSuspended;

            source.Resuming += OnResuming;
            source.Resumed += OnResumed;

            source.Destroying += OnDestroying;
            source.Destroyed += OnDestroyed;
        }

        public void Destroy()
        {
            source.StateChanged -= OnStateChanged;

            source.Initializing -= OnInitializing;
            source.Initialized -= OnInitialized;

            source.Suspending -= OnSuspending;
            source.Suspended -= OnSuspended;

            source.Resuming -= OnResuming;
            source.Resumed -= OnResumed;

            source.Destroying -= OnDestroying;
            source.Destroyed -= OnDestroyed;

            source = null;
        }

        private static void OnStateChanged()
        {
            LifecycleEvent.Dispatch(LifecycleEvent.Type.StateChanged);
        }

        private static void OnInitializing(object target)
        {
            LifecycleEvent.Dispatch(LifecycleEvent.Type.Initializing);
        }

        private static void OnInitialized(object target)
        {
            LifecycleEvent.Dispatch(LifecycleEvent.Type.Initialized);
        }

        private static void OnSuspending(object target)
        {
            LifecycleEvent.Dispatch(LifecycleEvent.Type.Suspending);
        }

        private static void OnSuspended(object target)
        {
            LifecycleEvent.Dispatch(LifecycleEvent.Type.Suspended);
        }

        private static void OnResuming(object target)
        {
            LifecycleEvent.Dispatch(LifecycleEvent.Type.Resuming);
        }

        private static void OnResumed(object target)
        {
            LifecycleEvent.Dispatch(LifecycleEvent.Type.Resumed);
        }

        private static void OnDestroying(object target)
        {
            LifecycleEvent.Dispatch(LifecycleEvent.Type.Destroying);
        }

        private static void OnDestroyed(object target)
        {
            LifecycleEvent.Dispatch(LifecycleEvent.Type.Destroyed);
        }
    }
}