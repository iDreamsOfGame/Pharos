using Pharos.Framework;

namespace Pharos.Extensions.CommandManagement
{
    public class CommandManagementExtension : IExtension
    {
        public void Enable(IContext context)
        {
            context.Injector.Map<IEventCommandMap>().ToSingleton<EventCommandMap>();
        }

        public void Disable(IContext context)
        {
        }
    }
}