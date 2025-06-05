using Pharos.Common.ViewCenter;

namespace Pharos.Extensions.Mediation
{
    public abstract class Mediator : IMediator
    {
        IView IMediator.View
        {
            get => View;
            set => View = value;
        }

        protected IView View { get; private set; }

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
    }
}