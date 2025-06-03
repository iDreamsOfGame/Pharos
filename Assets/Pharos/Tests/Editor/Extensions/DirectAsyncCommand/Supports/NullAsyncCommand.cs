using Pharos.Common.CommandCenter;

namespace PharosEditor.Tests.Extensions.DirectAsyncCommand.Supports
{
    internal class NullAsyncCommand : AsyncCommand
    {
        public override void Execute()
        {
            Executed();
        }
    }
}