using Pharos.Framework;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class HappyGuard : IGuard
    {
        public bool Approve() => true;
    }
}