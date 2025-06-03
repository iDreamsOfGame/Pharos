using System;

namespace Pharos.Common.CommandCenter
{
    public abstract class AsyncCommand : IAsyncCommand
    {
        private Action<IAsyncCommand, bool> executedCallback;

        Action<IAsyncCommand, bool> IAsyncCommand.ExecutedCallback
        {
            set => executedCallback = value;
        }
        
        public abstract void Execute();

        public virtual void Abort()
        {
            Executed(true);
        }
        
        protected virtual void Executed(bool stop = false)
        {
            executedCallback?.Invoke(this, stop);
        }
    }
}