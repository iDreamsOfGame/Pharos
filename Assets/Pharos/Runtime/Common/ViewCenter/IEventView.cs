using Pharos.Common.EventCenter;

namespace Pharos.Common.ViewCenter
{
    public interface IEventView : IView
    {
        bool ViewDispatcherCacheEnabled { get; }

        IEventDispatcher ViewDispatcher { get; set; }
    }
}