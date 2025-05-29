namespace Pharos.Extensions.DirectCommand
{
    public interface IDirectCommandConfigurator : IDirectCommandMapper
    {
        IDirectCommandConfigurator WithGuards(params object[] guards);

        IDirectCommandConfigurator WithHooks(params object[] hooks);

        IDirectCommandConfigurator WithPayloadInjection(bool value = true);
    }
}