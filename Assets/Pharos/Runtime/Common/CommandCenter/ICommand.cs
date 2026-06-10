using UnityEngine.Scripting;

namespace Pharos.Common.CommandCenter
{
    [RequireImplementors]
    public interface ICommand
    {
        void Execute();
    }
}