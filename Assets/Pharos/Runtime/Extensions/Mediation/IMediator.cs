using Pharos.Common.EventCenter;
using Pharos.Common.ViewCenter;

namespace Pharos.Extensions.Mediation
{
    public interface IMediator
    {
        public IEventDispatcher EventDispatcher { get; }

        IView View { get; internal set; }

        IEventDispatcher ViewDispatcher { get; internal set; }

        internal void PreInitialize();
        
        internal void Initialize();

        internal void PostInitialize();

        internal void PreDestroy();
        
        internal void Destroy();

        internal void PostDestroy();
    }
}