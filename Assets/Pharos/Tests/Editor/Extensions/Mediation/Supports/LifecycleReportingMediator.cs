using System;
using Pharos.Common.EventCenter;
using Pharos.Common.ViewCenter;
using Pharos.Extensions.Mediation;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Extensions.Mediation.Supports
{
    internal class LifecycleReportingMediator : IMediator
    {
        [Inject(true, nameof(PreInitializeCallback))]
        public Action<string> PreInitializeCallback { get; private set; }

        [Inject(true, nameof(InitializeCallback))]
        public Action<string> InitializeCallback { get; private set; }

        [Inject(true, nameof(PostInitializeCallback))]
        public Action<string> PostInitializeCallback { get; private set; }

        [Inject(true, nameof(PreDestroyCallback))]
        public Action<string> PreDestroyCallback { get; private set; }

        [Inject(true, nameof(DestroyCallback))]
        public Action<string> DestroyCallback { get; private set; }

        [Inject(true, nameof(PostDestroyCallback))]
        public Action<string> PostDestroyCallback { get; private set; }

        IView IMediator.View { get; set; }

        IEventDispatcher IMediator.ViewDispatcher { get; set; }

        void IMediator.PreInitialize()
        {
            PreInitializeCallback?.Invoke(nameof(PreInitializeCallback));
        }

        void IMediator.Initialize()
        {
            InitializeCallback?.Invoke(nameof(InitializeCallback));
        }

        void IMediator.PostInitialize()
        {
            PostInitializeCallback?.Invoke(nameof(PostInitializeCallback));
        }

        void IMediator.PreDestroy()
        {
            PreDestroyCallback?.Invoke(nameof(PreDestroyCallback));
        }

        void IMediator.Destroy()
        {
            DestroyCallback?.Invoke(nameof(DestroyCallback));
        }

        void IMediator.PostDestroy()
        {
            PostDestroyCallback?.Invoke(nameof(PostDestroyCallback));
        }
    }
}