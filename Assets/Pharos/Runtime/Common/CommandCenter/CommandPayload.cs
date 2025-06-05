using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharos.Common.CommandCenter
{
    public struct CommandPayload
    {
        public CommandPayload(Dictionary<object, Type> valueToType = null)
        {
            ValueToType = valueToType;
        }

        public Dictionary<object, Type> ValueToType { get; private set; }

        public List<Type> Types => ValueToType?.Values.ToList();

        public List<object> Values => ValueToType?.Keys.ToList();

        public bool HasPayload => ValueToType is { Count: > 0 };

        public CommandPayload AddPayload<T>(object value)
        {
            return AddPayload(value, typeof(T));
        }

        public CommandPayload AddPayload(object value, Type type)
        {
            ValueToType ??= new Dictionary<object, Type>();
            ValueToType.Add(value, type);
            return this;
        }
    }
}