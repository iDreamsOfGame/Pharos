using System;
using Pharos.Common.EventCenter;
using Pharos.Common.ViewCenter;
using Pharos.Extensions.Mediation;
using VContainer;

namespace PharosEditor.Tests.Extensions.Mediation.Supports
{
    [InjectIgnore]
    internal class LifecycleReportingMediator : IMediator
    {
        [Inject, Key(nameof(PreInitializeCallback))]
        public Action<string> PreInitializeCallback { get; private set; }

        [Inject, Key(nameof(InitializeCallback))]
        public Action<string> InitializeCallback { get; private set; }

        [Inject, Key(nameof(PostInitializeCallback))]
        public Action<string> PostInitializeCallback { get; private set; }

        [Inject, Key(nameof(PreDestroyCallback))]
        public Action<string> PreDestroyCallback { get; private set; }

        [Inject, Key(nameof(DestroyCallback))]
        public Action<string> DestroyCallback { get; private set; }

        [Inject, Key(nameof(PostDestroyCallback))]
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