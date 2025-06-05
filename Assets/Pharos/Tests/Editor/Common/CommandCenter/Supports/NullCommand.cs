using Pharos.Common.CommandCenter;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class NullCommand : ICommand
    {
        public void Execute()
        {
        }
    }
}