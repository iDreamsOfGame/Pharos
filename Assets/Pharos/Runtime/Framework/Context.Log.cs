namespace Pharos.Framework
{
    public partial class Context
    {
        public LogLevel LogLevel
        {
            get => logManager?.LogLevel ?? LogLevel.Debug;
            set
            {
                if (logManager != null) 
                    logManager.LogLevel = value;
            }
        }

        public ILogger GetLogger(object source) => logManager?.GetLogger(source);

        public IContext AddLogHandler(ILogHandler handler)
        {
            logManager?.AddLogHandler(handler);
            return this;
        }
    }
}