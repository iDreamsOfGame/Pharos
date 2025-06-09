using System;
using System.Collections.Generic;
using Pharos.Common.CommandCenter;
using Pharos.Common.EventCenter;
using Pharos.Framework;
using Pharos.Framework.Injection;

namespace Pharos.Extensions.CommandManagement
{
    public class EventCommandMap : IEventCommandMap
    {
        private readonly List<Action<ICommandMapping>> mappingProcessors = new();

        private readonly IInjector injector;

        private readonly IEventDispatcher dispatcher;

        private readonly CommandTriggerMap triggerMap;

        private readonly ILogger logger;

        public EventCommandMap(IContext context, IEventDispatcher dispatcher)
        {
            injector = context.Injector;
            logger = context.GetLogger(this);
            this.dispatcher = dispatcher;
            triggerMap = new CommandTriggerMap(GetKey, CreateTrigger);
        }

        public ICommandMapper Map(Enum type, Type eventType = null)
        {
            return GetTrigger(type, eventType).CreateMapper();
        }

        public ICommandMapper Map<T>(Enum type)
        {
            return Map(type, typeof(T));
        }

        public ICommandUnmapper Unmap(Enum type, Type eventType = null)
        {
            return GetTrigger(type, eventType).CreateMapper();
        }

        public ICommandUnmapper Unmap<T>(Enum type)
        {
            return Unmap(type, typeof(T));
        }

        public IEventCommandMap AddMappingProcessor(Action<ICommandMapping> processor)
        {
            if (!mappingProcessors.Contains(processor))
                mappingProcessors.Add(processor);

            return this;
        }

        private static object GetKey(params object[] args)
        {
            var type = args[0] as Enum;
            return args[1] is Type eventType ? new Tuple<object, Type>(type, eventType) : type;
        }

        private EventCommandTrigger GetTrigger(Enum type, Type eventType)
        {
            return triggerMap.GetTrigger(type, eventType) as EventCommandTrigger;
        }

        private ICommandTrigger CreateTrigger(params object[] args)
        {
            var type = args[0] as Enum;
            var eventType = args[1] as Type;
            return new EventCommandTrigger(injector, dispatcher, type, eventType, mappingProcessors, logger);
        }
    }
}