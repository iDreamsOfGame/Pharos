using System;

namespace Pharos.Common.CommandCenter
{
    public interface IAsyncCommand : ICommand
    {
        internal Action<IAsyncCommand, bool> ExecutedCallback { set; }
        
        void Abort();
    }
}