using System;
using Pharos.Common.CommandCenter;

namespace Pharos.Extensions.CommandManagement
{
    public interface IEventCommandMap
    {
        ICommandMapper Map(Enum type, Type eventType = null);

        ICommandMapper Map<T>(Enum type);

        ICommandUnmapper Unmap(Enum type, Type eventType = null);

        ICommandUnmapper Unmap<T>(Enum type);

        IEventCommandMap AddMappingProcessor(Action<ICommandMapping> processor);
    }
}