using System;
using UnityEngine.Scripting;

namespace Pharos.Common.CommandCenter
{
    [RequireImplementors]
    public interface IAsyncCommand : ICommand
    {
        internal Action<IAsyncCommand, bool> ExecutedCallback { set; }
        
        void Abort();
    }
}