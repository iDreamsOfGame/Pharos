using System;
using Pharos.Common.ViewCenter;

namespace Pharos.Extensions.Mediation
{
    public interface IMediatorManager
    {
        IMediator GetMediator(IView view);

        IMediator CreateMediator(IView view, Type viewType, IMediatorMapping mapping);
    }
}