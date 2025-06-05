using System.Collections.Generic;

namespace Pharos.Common.CommandCenter
{
    public class CommandTriggerMap
    {
        public delegate object KeyFactory(params object[] args);

        public delegate ICommandTrigger TriggerFactory(params object[] args);

        private readonly Dictionary<object, ICommandTrigger> keyToTrigger = new();

        private readonly KeyFactory keyFactory;

        private readonly TriggerFactory triggerFactory;

        public CommandTriggerMap(KeyFactory keyFactory, TriggerFactory triggerFactory)
        {
            this.keyFactory = keyFactory;
            this.triggerFactory = triggerFactory;
        }

        public ICommandTrigger GetTrigger(params object[] args)
        {
            var key = GetKey(args);
            return keyToTrigger.TryGetValue(key, out var trigger) ? trigger : keyToTrigger[key] = CreateTrigger(args);
        }

        public ICommandTrigger RemoveTrigger(params object[] args)
        {
            return DestroyTrigger(GetKey(args));
        }

        private object GetKey(object[] args)
        {
            return keyFactory?.Invoke(args);
        }

        private ICommandTrigger CreateTrigger(object[] args)
        {
            return triggerFactory?.Invoke(args);
        }

        private ICommandTrigger DestroyTrigger(object key)
        {
            if (!keyToTrigger.TryGetValue(key, out var trigger))
                return null;

            trigger.Deactivate();
            keyToTrigger.Remove(key);
            return trigger;
        }
    }
}