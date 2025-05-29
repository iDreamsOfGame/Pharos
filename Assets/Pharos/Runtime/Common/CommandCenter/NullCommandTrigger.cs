namespace Pharos.Common.CommandCenter
{
    public class NullCommandTrigger : ICommandTrigger
    {
        private static ICommandTrigger instance;

        public static ICommandTrigger Instance => instance ??= new NullCommandTrigger();

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }
    }
}