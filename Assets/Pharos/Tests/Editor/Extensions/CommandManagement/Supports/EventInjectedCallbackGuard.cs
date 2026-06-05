using System;
using Pharos.Common.EventCenter;
using Pharos.Framework;
using VContainer;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    [InjectIgnore]
    internal class EventInjectedCallbackGuard : IGuard
    {
        [Inject]
        public IEvent Event { get; private set; }

        [Inject, Key("ApproveCallback")]
        public Action<EventInjectedCallbackGuard> Callback { get; private set; }

        public bool Approve()
        {
            Callback?.Invoke(this);
            return true;
        }
    }
}