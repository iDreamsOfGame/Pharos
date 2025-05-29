using Pharos.Framework;

namespace Pharos.Extensions.DirectCommand
{
    public class DirectCommandExtension : IExtension
    {
        public void Enable(IContext context)
        {
            context.Injector.Map<IDirectCommandMap>().ToType<DirectCommandMap>();
        }

        public void Disable(IContext context)
        {
        }
    }
}