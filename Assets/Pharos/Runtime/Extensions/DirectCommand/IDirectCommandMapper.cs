using System;
using Pharos.Common.CommandCenter;

namespace Pharos.Extensions.DirectCommand
{
    public interface IDirectCommandMapper
    {
        IDirectCommandConfigurator Map(Type commandType);

        IDirectCommandConfigurator Map<T>();

        void Execute(CommandPayload payload = default);
    }
}