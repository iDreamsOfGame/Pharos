namespace Pharos.Framework
{
    public interface IExtension
    {
        void Enable(IContext context);

        void Disable(IContext context);
    }
}