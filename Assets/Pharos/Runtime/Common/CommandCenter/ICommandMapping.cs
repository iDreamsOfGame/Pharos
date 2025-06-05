using System;
using System.Collections.Generic;
using Pharos.Framework;

namespace Pharos.Common.CommandCenter
{
    public interface ICommandMapping
    {
        Type CommandType { get; }

        List<Type> GuardTypes { get; }

        List<Type> HookTypes { get; }

        bool ShouldExecuteOnce { get; set; }

        bool PayloadInjectionEnabled { get; set; }

        ICommandMapping AddGuard<T>() where T : IGuard;

        ICommandMapping AddGuards<T1, T2>()
            where T1 : IGuard
            where T2 : IGuard;

        ICommandMapping AddGuards<T1, T2, T3>()
            where T1 : IGuard
            where T2 : IGuard
            where T3 : IGuard;

        ICommandMapping AddGuards<T1, T2, T3, T4>()
            where T1 : IGuard
            where T2 : IGuard
            where T3 : IGuard
            where T4 : IGuard;

        ICommandMapping AddGuards<T1, T2, T3, T4, T5>()
            where T1 : IGuard
            where T2 : IGuard
            where T3 : IGuard
            where T4 : IGuard
            where T5 : IGuard;

        ICommandMapping AddGuards(params Type[] guards);

        ICommandMapping AddHook<T>() where T : IHook;

        ICommandMapping AddHooks<T1, T2>()
            where T1 : IHook
            where T2 : IHook;

        ICommandMapping AddHooks<T1, T2, T3>()
            where T1 : IHook
            where T2 : IHook
            where T3 : IHook;

        ICommandMapping AddHooks<T1, T2, T3, T4>()
            where T1 : IHook
            where T2 : IHook
            where T3 : IHook
            where T4 : IHook;

        ICommandMapping AddHooks<T1, T2, T3, T4, T5>()
            where T1 : IHook
            where T2 : IHook
            where T3 : IHook
            where T4 : IHook
            where T5 : IHook;

        ICommandMapping AddHooks(params Type[] hooks);
    }
}