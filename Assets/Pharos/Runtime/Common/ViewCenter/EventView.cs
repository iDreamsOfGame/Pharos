using Pharos.Common.EventCenter;

namespace Pharos.Common.ViewCenter
{
    public abstract class EventView : View, IEventView
    {
        public virtual bool ViewDispatcherCacheEnabled => true;

        IEventDispatcher IEventView.ViewDispatcher
        {
            get => ViewDispatcher;
            set => ViewDispatcher = value;
        }

        protected IEventDispatcher ViewDispatcher { get; private set; }
    }
}