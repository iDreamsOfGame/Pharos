using Pharos.Common;

namespace Pharos.Extensions.Mediation
{
    public interface IMediatorConfigurator : IConfigurator<IMediatorConfigurator>
    {
        IMediatorConfigurator EnableAutoDestroy();

        IMediatorConfigurator DisableAutoDestroy();
    }
}