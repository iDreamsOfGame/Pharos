using System;
using Pharos.Common.ViewCenter;

namespace Pharos.Extensions.Mediation
{
    public interface IMediatorManager
    {
        IMediator CreateMediator(IView view, Type viewType, IMediatorMapping mapping);
        
        IMediator GetMediator(IView view);

        void DestroyMediator(IView view);
    }
}