using System;
using System.Collections.Generic;

namespace Pharos.Common.CommandCenter
{
    public class CommandMapping : ICommandMapping
    {
        public CommandMapping(Type commandType)
        {
            CommandType = commandType;
        }

        public Type CommandType { get; }

        public List<object> Guards { get; private set; }

        public List<object> Hooks { get; private set; }

        public bool ShouldExecuteOnce { get; set; }

        public bool PayloadInjectionEnabled { get; set; } = true;

        public ICommandMapping AddGuards(params object[] guards)
        {
            Guards ??= new List<object>();
            Guards.AddRange(guards);
            return this;
        }

        public ICommandMapping AddGuards<T>()
        {
            return AddGuards(typeof(T));
        }

        public ICommandMapping AddGuards<T1, T2>()
        {
            return AddGuards(typeof(T1), typeof(T2));
        }

        public ICommandMapping AddGuards<T1, T2, T3>()
        {
            return AddGuards(typeof(T1), typeof(T2), typeof(T3));
        }

        public ICommandMapping AddGuards<T1, T2, T3, T4>()
        {
            return AddGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public ICommandMapping AddGuards<T1, T2, T3, T4, T5>()
        {
            return AddGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        public ICommandMapping AddHooks(params object[] hooks)
        {
            Hooks ??= new List<object>();
            Hooks.AddRange(hooks);
            return this;
        }

        public ICommandMapping AddHooks<T>()
        {
            return AddHooks(typeof(T));
        }

        public ICommandMapping AddHooks<T1, T2>()
        {
            return AddHooks(typeof(T1), typeof(T2));
        }

        public ICommandMapping AddHooks<T1, T2, T3>()
        {
            return AddHooks(typeof(T1), typeof(T2), typeof(T3));
        }

        public ICommandMapping AddHooks<T1, T2, T3, T4>()
        {
            return AddGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public ICommandMapping AddHooks<T1, T2, T3, T4, T5>()
        {
            return AddGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }
    }
}