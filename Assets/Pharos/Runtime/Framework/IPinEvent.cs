using System;

namespace Pharos.Framework
{
    public interface IPinEvent
    {
        event Action<object> Detained;

        event Action<object> Released;
    }
}