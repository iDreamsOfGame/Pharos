using System;

namespace Pharos.Common.ViewCenter
{
    public interface IView
    {
        event Action<IView> Destroying;
    }
}