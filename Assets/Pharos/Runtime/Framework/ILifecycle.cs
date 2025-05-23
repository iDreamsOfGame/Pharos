using System;

namespace Pharos.Framework
{
    public interface ILifecycle : ILifecycleEvent
    {
        /// <summary>
        /// The current lifecycle state of the target object. 
        /// </summary>
        LifecycleState State { get; }

        /// <summary>
        /// Gets a value that indicates whether this object has been fully initialized or not.
        /// </summary>
        bool HasInitialized { get; }

        /// <summary>
        /// Gets a value that indicates whether this object currently has activated or not. 
        /// </summary>
        bool HasActivated { get; }

        /// <summary>
        /// Gets a value that indicates whether this object has been fully suspended or not. 
        /// </summary>
        bool HasSuspended { get; }
        
        /// <summary>
        /// Gets a value that indicates whether this object has been fully destroyed or not. 
        /// </summary>
        bool HasDestroyed { get; }

        /// <summary>
        /// Initializes the lifecycle. 
        /// </summary>
        /// <param name="callback"></param>
        void Initialize(Action<Exception> callback = null);
        
        /// <summary>
        /// Suspends the lifecycle. 
        /// </summary>
        /// <param name="callback"></param>
        void Suspend(Action<Exception> callback = null);
        
        /// <summary>
        /// Resumes a suspended lifecycle. 
        /// </summary>
        /// <param name="callback"></param>
        void Resume(Action<Exception> callback = null);
        
        /// <summary>
        /// Destroys an activated lifecycle. 
        /// </summary>
        /// <param name="callback"></param>
        void Destroy(Action<Exception> callback = null);
    }
}