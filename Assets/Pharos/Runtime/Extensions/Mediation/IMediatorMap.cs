using System;
using Pharos.Common.ViewCenter;

namespace Pharos.Extensions.Mediation
{
    public interface IMediatorMap
    {
        IMediatorMapper Map<T>() where T : IView;
        
        IMediatorMapper Map(Type viewType);
        
        IMediatorUnmapper Unmap<T>() where T : IView;
        
        IMediatorUnmapper Unmap(Type viewType);
    }
}