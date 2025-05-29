namespace Pharos.Common.CommandCenter
{
    public interface ICommandConfigurator
    {
        ICommandConfigurator WithGuards(params object[] guards);

        ICommandConfigurator WithGuards<T>();

        ICommandConfigurator WithGuards<T1, T2>();

        ICommandConfigurator WithGuards<T1, T2, T3>();

        ICommandConfigurator WithGuards<T1, T2, T3, T4>();

        ICommandConfigurator WithGuards<T1, T2, T3, T4, T5>();

        ICommandConfigurator WithHooks(params object[] hooks);

        ICommandConfigurator WithHooks<T>();

        ICommandConfigurator WithHooks<T1, T2>();

        ICommandConfigurator WithHooks<T1, T2, T3>();

        ICommandConfigurator WithHooks<T1, T2, T3, T4>();

        ICommandConfigurator WithHooks<T1, T2, T3, T4, T5>();

        ICommandConfigurator ExecuteOnce(bool value = true);

        ICommandConfigurator WithPayloadInjection(bool value);
    }
}