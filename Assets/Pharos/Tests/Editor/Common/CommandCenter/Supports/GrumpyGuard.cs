using Pharos.Framework;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class GrumpyGuard : IGuard
    {
        public bool Approve() => false;
    }
}