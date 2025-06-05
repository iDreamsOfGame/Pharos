using System;

namespace Pharos.Common.ViewCenter
{
    public interface IViewHandler
    {
        void HandleViewInitialized(IView view, Type viewType);
    }
}