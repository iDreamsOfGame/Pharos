namespace Pharos.Common.CommandCenter
{
    public interface ICommandTrigger
    {
        void Activate();

        void Deactivate();
    }
}