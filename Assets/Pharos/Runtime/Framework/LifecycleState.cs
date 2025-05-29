namespace Pharos.Framework
{
    public enum LifecycleState
    {
        UnInitialized,

        Initializing,

        Activated,

        Suspending,

        Suspended,

        Resuming,

        Destroying,

        Destroyed
    }
}