using UnityEngine.Scripting;

namespace Pharos.Framework
{
    [RequireImplementors]
    public interface IExtension
    {
        void Enable(IContext context);

        void Disable(IContext context);
    }
}