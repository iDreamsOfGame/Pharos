namespace Pharos.Common.CommandCenter
{
    public interface ICommandConfigurator : IConfigurator<ICommandConfigurator>
    {
        ICommandConfigurator ExecuteOnce(bool value = true);

        ICommandConfigurator WithPayloadInjection(bool value);
    }
}