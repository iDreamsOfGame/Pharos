using System;
using Pharos.Extensions.Mediation;
using VContainer;

namespace PharosEditor.Tests.Extensions.Mediation.Supports
{
    [InjectIgnore]
    internal class CallbackMediator : Mediator
    {
        [Inject, Key("callback")]
        private Action<object> initializedCallback;

        [Inject, Key("destroyedCallback")]
        private Action<object> destroyedCallback;

        protected override void OnInitialized()
        {
            initializedCallback?.Invoke(this);
        }

        protected override void OnDestroyed()
        {
            destroyedCallback?.Invoke(this);
        }
    }
}