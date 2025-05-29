using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharos.Common.CommandCenter
{
    public struct CommandPayload
    {
        public CommandPayload(Dictionary<object, Type> valueTypeMap = null)
        {
            ValueTypeMap = valueTypeMap;
        }

        public Dictionary<object, Type> ValueTypeMap { get; private set; }

        public List<Type> Types => ValueTypeMap?.Values.ToList();

        public List<object> Values => ValueTypeMap?.Keys.ToList();

        public bool HasPayload => ValueTypeMap is { Count: > 0 };

        public CommandPayload AddPayload<T>(object value)
        {
            return AddPayload(value, typeof(T));
        }

        public CommandPayload AddPayload(object value, Type type)
        {
            ValueTypeMap ??= new Dictionary<object, Type>();
            ValueTypeMap.Add(value, type);
            return this;
        }
    }
}