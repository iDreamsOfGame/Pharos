using Pharos.Framework;

namespace Pharos.Extensions.DirectAsyncCommand
{
    public class DirectAsyncCommandMapExtension : IExtension
    {
        public void Enable(IContext context)
        {
            context.Injector.Map<IDirectAsyncCommandMap>().ToType<DirectAsyncCommandMap>();
        }

        public void Disable(IContext context)
        {
        }
    }
}