using System;
using System.Collections.Generic;
using Pharos.Common.CommandCenter;
using Pharos.Extensions.EventManagement;
using Pharos.Framework;
using Pharos.Framework.Injection;

namespace Pharos.Extensions.CommandManagement
{
    public class EventCommandTrigger : ICommandTrigger
    {
        private readonly IEventDispatcher dispatcher;

        private readonly Enum type;

        private readonly Type eventType;

        private readonly ICommandMappingList mappings;

        private readonly ICommandExecutor executor;
        
        public EventCommandTrigger(IInjector injector,
            IEventDispatcher dispatcher,
            Enum type,
            Type eventType = null,
            IEnumerable<Action<ICommandMapping>> processors = null,
            ILogger logger = null)
        {
            this.dispatcher = dispatcher;
            this.type = type;
            this.eventType = eventType;
            mappings = new CommandMappingList(this, processors, logger);
            executor = new CommandExecutor(injector, mappings.RemoveMapping);
        }

        public CommandMapper CreateMapper()
        {
            return new CommandMapper(mappings);
        }
        
        public void Activate()
        {
            dispatcher.AddEventListener(type, EventHandler);
        }

        public void Deactivate()
        {
            dispatcher.RemoveEventListener(type, EventHandler);
        }

        public override string ToString()
        {
            return $"{eventType.FullName} with selector \"{type}\"";
        }

        private void EventHandler(IEvent e)
        {
            var targetEventType = e.GetType();
            Type payloadEventType = null;
            if (targetEventType == eventType || eventType == null)
            {
                payloadEventType = targetEventType == typeof(Event) ? typeof(IEvent) : targetEventType;
            }
            else if (eventType == typeof(IEvent))
            {
                payloadEventType = eventType;
            }

            if (payloadEventType == null)
                return;

            var payload = new CommandPayload();
            payload.AddPayload(e, payloadEventType);
            executor?.ExecuteCommands(mappings.Mappings, payload);
        }
    }
}