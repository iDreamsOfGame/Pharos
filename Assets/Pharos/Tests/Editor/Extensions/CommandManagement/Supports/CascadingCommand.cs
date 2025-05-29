using Pharos.Extensions.CommandManagement;
using Pharos.Extensions.EventManagement;
using PharosEditor.Tests.Common.CommandCenter.Supports;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Extensions.CommandManagement.Supports
{
    public class CascadingCommand
    {
        public enum EventType
        {
            CascadingEvent
        }

        [Inject]
        private IEventDispatcher dispatcher;

        [Inject]
        private IEventCommandMap eventCommandMap;

        public void Execute()
        {
            eventCommandMap.Map(EventType.CascadingEvent).ToCommand<NullCommand>().ExecuteOnce();
            dispatcher.Dispatch(new Event(EventType.CascadingEvent));
        }
    }
}