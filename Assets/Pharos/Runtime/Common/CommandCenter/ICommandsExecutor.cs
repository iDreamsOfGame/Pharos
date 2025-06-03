using System.Collections.Generic;
using Pharos.Framework.Injection;

namespace Pharos.Common.CommandCenter
{
    public interface ICommandsExecutor
    {
        IInjector Injector { get; }

        void ExecuteCommand(ICommandMapping mapping, CommandPayload payload = default);

        void ExecuteCommands(IEnumerable<ICommandMapping> mappings, CommandPayload payload = default);
    }
}