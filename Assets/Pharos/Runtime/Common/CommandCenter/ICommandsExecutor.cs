using System.Collections.Generic;

namespace Pharos.Common.CommandCenter
{
    public interface ICommandsExecutor
    {
        void ExecuteCommand(ICommandMapping mapping, CommandPayload payload = default);

        void ExecuteCommands(IEnumerable<ICommandMapping> mappings, CommandPayload payload = default);
    }
}