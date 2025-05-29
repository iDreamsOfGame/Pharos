using System;

namespace Pharos.Framework.Helpers
{
    internal class LifecycleManager : ILifecycle
    {
        private LifecycleTransition initializingTransition;

        private LifecycleTransition suspendingTransition;

        private LifecycleTransition resumingTransition;

        private LifecycleTransition destroyingTransition;

        public LifecycleManager(object target)
        {
            Target = target;
            ConfigureTransitions();
        }

        public event Action<Exception> ErrorOccurred;

        public event Action StateChanged;

        public event Action<object> Initializing;

        public event Action<object> Initialized;

        public event Action<object> Suspending;

        public event Action<object> Suspended;

        public event Action<object> Resuming;

        public event Action<object> Resumed;

        public event Action<object> Destroying;

        public event Action<object> Destroyed;

        public object Target { get; }

        public LifecycleState State { get; private set; }

        public bool HasInitialized => State != LifecycleState.UnInitialized && State != LifecycleState.Initializing;

        public bool HasActivated => State == LifecycleState.Activated;

        public bool HasSuspended => State == LifecycleState.Suspended;

        public bool HasDestroyed => State == LifecycleState.Destroyed;

        public void Initialize(Action<Exception> callback = null)
        {
            initializingTransition.Enter(callback);
        }

        public void Suspend(Action<Exception> callback = null)
        {
            suspendingTransition.Enter(callback);
        }

        public void Resume(Action<Exception> callback = null)
        {
            resumingTransition.Enter(callback);
        }

        public void Destroy(Action<Exception> callback = null)
        {
            destroyingTransition.Enter(callback);
        }

        internal void SetState(LifecycleState state)
        {
            if (State == state)
                return;

            State = state;
            StateChanged?.Invoke();
        }

        internal bool ReportError(Exception exception)
        {
            var invocationList = ErrorOccurred?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (invocationList.Length > 0)
            {
                ErrorOccurred?.Invoke(exception);
                return true;
            }

            return false;
        }

        private void ConfigureTransitions()
        {
            initializingTransition = new LifecycleTransition(this)
                .FromStates(LifecycleState.UnInitialized)
                .ToStates(LifecycleState.Initializing, LifecycleState.Activated);
            initializingTransition.ProcessingCallback = OnInitializing;
            initializingTransition.PostprocessCallback = OnInitialized;

            suspendingTransition = new LifecycleTransition(this)
                .FromStates(LifecycleState.Activated)
                .ToStates(LifecycleState.Suspending, LifecycleState.Suspended);
            suspendingTransition.ProcessingCallback = OnSuspending;
            suspendingTransition.PostprocessCallback = OnSuspended;

            resumingTransition = new LifecycleTransition(this)
                .FromStates(LifecycleState.Suspended)
                .ToStates(LifecycleState.Resuming, LifecycleState.Activated);
            resumingTransition.ProcessingCallback = OnResuming;
            resumingTransition.PostprocessCallback = OnResumed;

            destroyingTransition = new LifecycleTransition(this)
                .FromStates(LifecycleState.Suspended, LifecycleState.Activated)
                .ToStates(LifecycleState.Destroying, LifecycleState.Destroyed);
            destroyingTransition.ProcessingCallback = OnDestroying;
            destroyingTransition.PostprocessCallback = OnDestroyed;
        }

        private void OnInitializing()
        {
            Initializing?.Invoke(Target);
        }

        private void OnInitialized()
        {
            Initialized?.Invoke(Target);
        }

        private void OnSuspending()
        {
            Suspending?.Invoke(Target);
        }

        private void OnSuspended()
        {
            Suspended?.Invoke(Target);
        }

        private void OnResuming()
        {
            Resuming?.Invoke(Target);
        }

        private void OnResumed()
        {
            Resumed?.Invoke(Target);
        }

        private void OnDestroying()
        {
            Destroying?.Invoke(Target);
        }

        private void OnDestroyed()
        {
            Destroyed?.Invoke(Target);
        }
    }
}