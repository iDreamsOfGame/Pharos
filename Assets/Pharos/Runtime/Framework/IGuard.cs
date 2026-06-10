using UnityEngine.Scripting;

namespace Pharos.Framework
{
    [RequireImplementors]
    public interface IGuard
    {
        bool Approve();
    }
}