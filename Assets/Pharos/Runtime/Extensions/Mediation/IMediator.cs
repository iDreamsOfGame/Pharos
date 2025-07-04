using Pharos.Common.EventCenter;
using Pharos.Common.ViewCenter;

namespace Pharos.Extensions.Mediation
{
    public interface IMediator
    {
        IView View { get; set; }

        IEventDispatcher ViewDispatcher { get; set; }

        internal void PreInitialize();
        
        internal void Initialize();

        internal void PostInitialize();

        internal void PreDestroy();
        
        internal void Destroy();

        internal void PostDestroy();
    }
}