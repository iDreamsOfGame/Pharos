using Pharos.Common.CommandCenter;

namespace PharosEditor.Tests.Extensions.DirectAsyncCommand.Supports
{
    internal class AbortAsyncCommand : AsyncCommand
    {
        public override void Execute()
        {
            Abort();
        }
    }
}