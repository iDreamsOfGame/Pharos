using Moq;
using Pharos.Common.CommandCenter;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class CommandMapStub
    {
        public virtual object KeyFactory(params object[] args)
        {
            var s = "";
            for (var i = 0; i < args.Length; i++)
            {
                s += args[i].ToString();
                if (i < args.Length - 1)
                    s += "::";
            }

            return s;
        }

        public virtual ICommandTrigger TriggerFactory(params object[] args)
        {
            return new Mock<ICommandTrigger>().Object;
        }

        public virtual void Hook(params object[] args)
        {
        }
    }
}