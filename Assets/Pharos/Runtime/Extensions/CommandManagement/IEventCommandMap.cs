using System;
using Pharos.Common.CommandCenter;
using UnityEngine.Scripting;

namespace Pharos.Extensions.CommandManagement
{
    [RequireImplementors]
    public interface IEventCommandMap
    {
        ICommandMapper Map(Enum type, Type eventType = null);

        ICommandMapper Map<T>(Enum type);

        ICommandUnmapper Unmap(Enum type, Type eventType = null);

        ICommandUnmapper Unmap<T>(Enum type);

        IEventCommandMap AddMappingProcessor(Action<ICommandMapping> processor);
    }
}