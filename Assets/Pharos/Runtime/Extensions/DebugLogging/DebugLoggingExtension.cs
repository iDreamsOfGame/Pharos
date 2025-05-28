using Pharos.Framework;

namespace Pharos.Extensions.DebugLogging
{
    public class DebugLoggingExtension : IExtension
    {
        public void Enable(IContext context)
        {
            context.AddLogHandler(new DebugLogHandler(context));
        }

        public void Disable(IContext context)
        {
        }
    }
}