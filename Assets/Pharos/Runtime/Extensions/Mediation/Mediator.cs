using System;
using Pharos.Common.EventCenter;
using Pharos.Common.ViewCenter;
using Pharos.Framework;
using ReflexPlus.Attributes;

namespace Pharos.Extensions.Mediation
{
    public abstract class Mediator : IMediator
    {
        [Inject(true)]
        public ILogger Logger { get; protected set; }

        [Inject(true)]
        public IEventDispatcher EventDispatcher { get; protected set; }

        public IView View { get; set; }

        public IEventDispatcher ViewDispatcher { get; set; }

        void IMediator.PreInitialize()
        {
            OnPreInitialize();
        }

        void IMediator.Initialize()
        {
            OnInitializing();
        }

        void IMediator.PostInitialize()
        {
            OnInitialized();
        }

        void IMediator.PreDestroy()
        {
            OnPreDestroy();
        }

        void IMediator.Destroy()
        {
            OnDestroying();
        }

        void IMediator.PostDestroy()
        {
            RemoveViewListeners();
            RemoveContextListeners();
            OnDestroyed();
        }

        protected virtual void OnPreInitialize()
        {
        }

        protected virtual void OnInitializing()
        {
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual void OnPreDestroy()
        {
        }

        protected virtual void OnDestroying()
        {
        }

        protected virtual void OnDestroyed()
        {
        }

        protected virtual void AddViewListener<T>(Enum type, Action<T> listener) where T : IEvent
        {
            if (ViewDispatcher == null)
            {
                TriggerViewDispatcherError();
                return;
            }

            ViewDispatcher.AddEventListener(type, listener);
        }

        protected virtual void AddViewListener(Enum type, Action<IEvent> listener)
        {
            if (ViewDispatcher == null)
            {
                TriggerViewDispatcherError();
                return;
            }

            ViewDispatcher.AddEventListener(type, listener);
        }

        protected virtual void AddViewListener(Enum type, Action listener)
        {
            if (ViewDispatcher == null)
            {
                TriggerViewDispatcherError();
                return;
            }

            ViewDispatcher.AddEventListener(type, listener);
        }

        protected virtual void AddViewListener(Enum type, Delegate listener)
        {
            if (ViewDispatcher == null)
            {
                TriggerViewDispatcherError();
                return;
            }

            ViewDispatcher.AddEventListener(type, listener);
        }

        protected virtual void RemoveViewListener<T>(Enum type, Action<T> listener) where T : IEvent
        {
            if (ViewDispatcher == null)
            {
                TriggerViewDispatcherError();
                return;
            }

            ViewDispatcher.RemoveEventListener(type, listener);
        }

        protected virtual void RemoveViewListener(Enum type, Action<IEvent> listener)
        {
            if (ViewDispatcher == null)
            {
                TriggerViewDispatcherError();
                return;
            }

            ViewDispatcher.RemoveEventListener(type, listener);
        }

        protected virtual void RemoveViewListener(Enum type, Action listener)
        {
            if (ViewDispatcher == null)
            {
                TriggerViewDispatcherError();
                return;
            }

            ViewDispatcher.RemoveEventListener(type, listener);
        }

        protected virtual void RemoveViewListener(Enum type, Delegate listener)
        {
            if (ViewDispatcher == null)
            {
                TriggerViewDispatcherError();
                return;
            }

            ViewDispatcher.RemoveEventListener(type, listener);
        }

        protected virtual void RemoveViewListeners()
        {
            if (ViewDispatcher == null)
            {
                TriggerViewDispatcherError();
                return;
            }

            ViewDispatcher.RemoveEventListeners(this);
        }

        protected virtual void AddContextListener<T>(Enum type, Action<T> listener) where T : IEvent
        {
            if (EventDispatcher == null)
            {
                TriggerEventDispatcherError();
                return;
            }
            
            EventDispatcher.AddEventListener(type, listener);
        }

        protected virtual void AddContextListener(Enum type, Action<IEvent> listener)
        {
            if (EventDispatcher == null)
            {
                TriggerEventDispatcherError();
                return;
            }
            
            EventDispatcher.AddEventListener(type, listener);
        }

        protected virtual void AddContextListener(Enum type, Action listener)
        {
            if (EventDispatcher == null)
            {
                TriggerEventDispatcherError();
                return;
            }
            
            EventDispatcher.AddEventListener(type, listener);
        }

        protected virtual void AddContextListener(Enum type, Delegate listener)
        {
            if (EventDispatcher == null)
            {
                TriggerEventDispatcherError();
                return;
            }
            
            EventDispatcher.AddEventListener(type, listener);
        }

        protected virtual void RemoveContextListener<T>(Enum type, Action<T> listener) where T : IEvent
        {
            if (EventDispatcher == null)
            {
                TriggerEventDispatcherError();
                return;
            }
            
            EventDispatcher.RemoveEventListener(type, listener);
        }

        protected virtual void RemoveContextListener(Enum type, Action<IEvent> listener)
        {
            if (EventDispatcher == null)
            {
                TriggerEventDispatcherError();
                return;
            }
            
            EventDispatcher.RemoveEventListener(type, listener);
        }

        protected virtual void RemoveContextListener(Enum type, Action listener)
        {
            if (EventDispatcher == null)
            {
                TriggerEventDispatcherError();
                return;
            }
            
            EventDispatcher.RemoveEventListener(type, listener);
        }

        protected virtual void RemoveContextListener(Enum type, Delegate listener)
        {
            if (EventDispatcher == null)
            {
                TriggerEventDispatcherError();
                return;
            }
            
            EventDispatcher.RemoveEventListener(type, listener);
        }

        protected virtual void RemoveContextListeners()
        {
            if (EventDispatcher == null)
            {
                TriggerEventDispatcherError();
                return;
            }
            
            EventDispatcher.RemoveEventListeners(this);
        }

        protected void Dispatch(IEvent e)
        {
            if (EventDispatcher == null)
            {
                TriggerEventDispatcherError();
                return;
            }
            
            if (EventDispatcher.HasEventListener(e.EventType))
                EventDispatcher.Dispatch(e);
        }

        private void TriggerViewDispatcherError()
        {
            if (Logger == null)
                return;

            if (View == null)
            {
                Logger.LogWarning("{0}: Can't add or remove view listeners because the {1} has not been set. ", this, nameof(View));
            }
            else
            {
                Logger.LogWarning("{0}: Can't add or remove view listeners because {1} is not, and does not contain, an {2}. ", this, View, nameof(IEventDispatcher));
            }
        }

        private void TriggerEventDispatcherError()
        {
            Logger?.LogWarning("{0}: Can't add or remove context listeners because the {1} has not been set. ", this, nameof(EventDispatcher));
        }
    }
}