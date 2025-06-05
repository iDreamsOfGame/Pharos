using System;
using System.Collections.Generic;
using Pharos.Framework;

namespace Pharos.Common.CommandCenter
{
    public class CommandMapping : ICommandMapping
    {
        public CommandMapping(Type commandType)
        {
            CommandType = commandType;
        }

        public Type CommandType { get; }

        public List<Type> GuardTypes { get; private set; }

        public List<Type> HookTypes { get; private set; }

        public bool ShouldExecuteOnce { get; set; }

        public bool PayloadInjectionEnabled { get; set; } = true;

        public ICommandMapping AddGuard<T>() where T : IGuard
        {
            return AddGuards(typeof(T));
        }

        public ICommandMapping AddGuards<T1, T2>()
            where T1 : IGuard
            where T2 : IGuard
        {
            return AddGuards(typeof(T1), typeof(T2));
        }

        public ICommandMapping AddGuards<T1, T2, T3>()
            where T1 : IGuard
            where T2 : IGuard
            where T3 : IGuard
        {
            return AddGuards(typeof(T1), typeof(T2), typeof(T3));
        }

        public ICommandMapping AddGuards<T1, T2, T3, T4>()
            where T1 : IGuard
            where T2 : IGuard
            where T3 : IGuard
            where T4 : IGuard
        {
            return AddGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public ICommandMapping AddGuards<T1, T2, T3, T4, T5>()
            where T1 : IGuard
            where T2 : IGuard
            where T3 : IGuard
            where T4 : IGuard
            where T5 : IGuard
        {
            return AddGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        public ICommandMapping AddGuards(params Type[] guards)
        {
            GuardTypes ??= new List<Type>();
            GuardTypes.AddRange(guards);
            return this;
        }

        public ICommandMapping AddHook<T>() where T : IHook
        {
            return AddHooks(typeof(T));
        }

        public ICommandMapping AddHooks<T1, T2>()
            where T1 : IHook
            where T2 : IHook
        {
            return AddHooks(typeof(T1), typeof(T2));
        }

        public ICommandMapping AddHooks<T1, T2, T3>()
            where T1 : IHook
            where T2 : IHook
            where T3 : IHook
        {
            return AddHooks(typeof(T1), typeof(T2), typeof(T3));
        }

        public ICommandMapping AddHooks<T1, T2, T3, T4>()
            where T1 : IHook
            where T2 : IHook
            where T3 : IHook
            where T4 : IHook
        {
            return AddGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public ICommandMapping AddHooks<T1, T2, T3, T4, T5>()
            where T1 : IHook
            where T2 : IHook
            where T3 : IHook
            where T4 : IHook
            where T5 : IHook
        {
            return AddGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        public ICommandMapping AddHooks(params Type[] hooks)
        {
            HookTypes ??= new List<Type>();
            HookTypes.AddRange(hooks);
            return this;
        }
    }
}