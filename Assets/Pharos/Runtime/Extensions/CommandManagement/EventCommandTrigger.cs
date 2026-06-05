using System;
using System.Collections.Generic;
using Pharos.Common.CommandCenter;
using Pharos.Common.EventCenter;
using Pharos.Framework;
using Pharos.Framework.Injection;

namespace Pharos.Extensions.CommandManagement
{
    public class EventCommandTrigger : ICommandTrigger
    {
        private readonly IContext context;
        
        private readonly IEventDispatcher dispatcher;
        
        private readonly IInjector sandboxInjector;

        private readonly Enum type;

        private readonly Type eventType;

        private readonly ICommandMappingList mappings;

        private readonly ICommandsExecutor executor;

        public EventCommandTrigger(IContext context,
            IEventDispatcher dispatcher,
            Enum type,
            Type eventType = null,
            IEnumerable<Action<ICommandMapping>> processors = null,
            ILogger logger = null)
        {
            this.context = context;
            this.dispatcher = dispatcher;
            this.type = type;
            this.eventType = eventType;
            sandboxInjector = context.Injector.CreateChild();
            mappings = new CommandMappingList(this, processors, logger);
            executor = new CommandsExecutor(sandboxInjector, mappings.RemoveMapping, null, null, OnCommandsExecuted);
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
            return eventType != null ? $"{eventType.FullName} with selector \"{type}\"" : string.Empty;
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

        private void OnCommandsExecuted()
        {
            context.Injector.RemoveChild(sandboxInjector);
        }
    }
}