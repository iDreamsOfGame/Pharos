using System.Collections.Generic;
using Pharos.Framework.Injection;

namespace Pharos.Common.CommandCenter
{
    public interface ICommandsExecutor
    {
        void ExecuteCommand(IPharosInjector injector, ICommandMapping mapping, CommandPayload payload = default);

        void ExecuteCommands(IPharosInjector injector, IEnumerable<ICommandMapping> mappings, CommandPayload payload = default);
    }
}