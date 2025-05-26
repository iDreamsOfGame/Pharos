using Pharos.Framework;

namespace Pharos.Extensions.EventManagement
{
    public class EventManagementExtension : IExtension
    {
        private readonly IEventDispatcher eventDispatcher;

        private LifecycleEventRelay lifecycleEventRelay;

        public EventManagementExtension(IEventDispatcher eventDispatcher = null)
        {
            this.eventDispatcher = eventDispatcher ?? new EventDispatcher();
            EventDispatcher.GlobalEventDispatcher = this.eventDispatcher;
        }

        public void Enable(IContext context)
        {
            context.Injector.Map<IEventDispatcher>().ToValue(eventDispatcher);
            lifecycleEventRelay = new LifecycleEventRelay(context);
        }

        public void Disable(IContext context)
        {
            lifecycleEventRelay?.Destroy();
            lifecycleEventRelay = null;
        }
    }
}