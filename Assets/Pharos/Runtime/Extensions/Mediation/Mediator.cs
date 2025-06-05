using System;
using Pharos.Common.EventCenter;
using Pharos.Common.ViewCenter;
using Pharos.Framework;
using ReflexPlus.Attributes;

namespace Pharos.Extensions.Mediation
{
    public abstract class Mediator : IMediator
    {
        [Inject]
        public ILogger Logger { get; protected set; }

        [Inject]
        public IEventDispatcher EventDispatcher { get; protected set; }

        IView IMediator.View
        {
            get => View;
            set => View = value;
        }

        IEventDispatcher IMediator.ViewDispatcher
        {
            get => ViewDispatcher;
            set => ViewDispatcher = value;
        }

        protected IView View { get; private set; }

        protected IEventDispatcher ViewDispatcher { get; private set; }

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
            OnPostInitialize();
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
            OnPostDestroy();
        }

        protected virtual void OnPreInitialize()
        {
        }

        protected virtual void OnInitializing()
        {
        }

        protected virtual void OnPostInitialize()
        {
        }

        protected virtual void OnPreDestroy()
        {
        }

        protected virtual void OnDestroying()
        {
        }

        protected virtual void OnPostDestroy()
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
            EventDispatcher.AddEventListener(type, listener);
        }
        
        protected virtual void AddContextListener(Enum type, Action<IEvent> listener)
        {
            EventDispatcher.AddEventListener(type, listener);
        }

        protected virtual void AddContextListener(Enum type, Action listener)
        {
            EventDispatcher.AddEventListener(type, listener);
        }

        protected virtual void AddContextListener(Enum type, Delegate listener)
        {
            EventDispatcher.AddEventListener(type, listener);
        }

        protected virtual void RemoveContextListener<T>(Enum type, Action<T> listener) where T : IEvent
        {
            EventDispatcher.RemoveEventListener(type, listener);
        }
        
        protected virtual void RemoveContextListener(Enum type, Action<IEvent> listener)
        {
            EventDispatcher.RemoveEventListener(type, listener);
        }

        protected virtual void RemoveContextListener(Enum type, Action listener)
        {
            EventDispatcher.RemoveEventListener(type, listener);
        }

        protected virtual void RemoveContextListener(Enum type, Delegate listener)
        {
            EventDispatcher.RemoveEventListener(type, listener);
        }

        protected virtual void RemoveContextListeners()
        {
            EventDispatcher.RemoveEventListeners(this);
        }

        protected void Dispatch(IEvent e)
        {
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
    }
}