using System;
using UnityEngine.Scripting;

namespace Pharos.Framework
{
    [RequireImplementors]
    public interface IPinEvent
    {
        event Action<object> Detained;

        event Action<object> Released;
    }
}