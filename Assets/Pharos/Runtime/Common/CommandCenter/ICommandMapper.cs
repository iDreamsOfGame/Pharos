using System;

namespace Pharos.Common.CommandCenter
{
    public interface ICommandMapper
    {
        ICommandConfigurator ToCommand<T>();

        ICommandConfigurator ToCommand(Type commandType);
    }
}