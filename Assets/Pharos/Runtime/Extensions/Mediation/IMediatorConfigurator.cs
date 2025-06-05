namespace Pharos.Extensions.Mediation
{
    public interface IMediatorConfigurator
    {
        IMediatorConfigurator WithGuards<T>();
        
        IMediatorConfigurator WithGuards<T1, T2>();
        
        IMediatorConfigurator WithGuards<T1, T2, T3>();
        
        IMediatorConfigurator WithGuards<T1, T2, T3, T4>();
        
        IMediatorConfigurator WithGuards<T1, T2, T3, T4, T5>();
        
        IMediatorConfigurator WithGuards(params object[] guards);
        
        IMediatorConfigurator WithHooks<T>();
        
        IMediatorConfigurator WithHooks<T1, T2>();
        
        IMediatorConfigurator WithHooks<T1, T2, T3>();
        
        IMediatorConfigurator WithHooks<T1, T2, T3, T4>();
        
        IMediatorConfigurator WithHooks<T1, T2, T3, T4, T5>();
        
        IMediatorConfigurator WithHooks(params object[] hooks);

        IMediatorConfigurator EnableAutoDestroy();

        IMediatorConfigurator DisableAutoDestroy();
    }
}