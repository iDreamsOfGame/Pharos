using System;

namespace Pharos.Extensions.Mediation
{
    public interface IMediatorMapper
    {
        IMediatorConfigurator ToMediator<T>() where T : IMediator;
        
        IMediatorConfigurator ToMediator(Type mediatorType);
    }
}