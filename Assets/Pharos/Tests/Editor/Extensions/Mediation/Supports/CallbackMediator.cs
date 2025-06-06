using System;
using Pharos.Extensions.Mediation;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Extensions.Mediation.Supports
{
    internal class CallbackMediator : Mediator
    {
        [Inject(true, "callback")]
        private Action<object> initializedCallback;

        [Inject(true, "destroyedCallback")]
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