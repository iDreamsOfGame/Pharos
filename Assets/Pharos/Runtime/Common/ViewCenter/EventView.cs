using Pharos.Common.EventCenter;

namespace Pharos.Common.ViewCenter
{
    public abstract class EventView : View, IEventView
    {
        public virtual bool ViewDispatcherCacheEnabled => true;

        public IEventDispatcher ViewDispatcher { get; set; }
    }
}