using System;
using Pharos.Common.EventCenter;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    internal class EventInjectedCallbackGuard
    {
        [Inject]
        public IEvent Event { get; private set; }

        [Inject("ApproveCallback")]
        public Action<EventInjectedCallbackGuard> Callback { get; private set; }

        public bool Approve()
        {
            Callback?.Invoke(this);
            return true;
        }
    }
}