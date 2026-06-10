using Pharos.Common.EventCenter;
using Pharos.Common.ViewCenter;
using UnityEngine.Scripting;

namespace Pharos.Extensions.Mediation
{
    [RequireImplementors]
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