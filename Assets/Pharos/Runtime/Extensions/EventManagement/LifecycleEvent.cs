using Pharos.Extensions.EventManagement.Pool;

namespace Pharos.Extensions.EventManagement
{
    public class LifecycleEvent : PooledEvent<LifecycleEvent>
    {
        public enum Type
        {
            ErrorOccurred,
            
            StateChanged,
            
            Initializing,
            
            Initialized,
            
            Suspending,
            
            Suspended,
            
            Resuming,
            
            Resumed,
            
            Destroying,
            
            Destroyed
        }
        
        public override void OnPreprocessGet()
        {
        }

        public override void OnPreprocessReturn()
        {
        }
    }
}