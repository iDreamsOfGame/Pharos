using System;

namespace Pharos.Framework
{
    public interface ILifecycleEvent
    {
        event Action<Exception> ErrorOccurred;

        event Action StateChanged;

        event Action<object> Initializing;

        event Action<object> Initialized;

        event Action<object> Suspending;

        event Action<object> Suspended;

        event Action<object> Resuming;

        event Action<object> Resumed;

        event Action<object> Destroying;

        event Action<object> Destroyed;
    }
}