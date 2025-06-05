using System;
using Pharos.Framework;

namespace Pharos.Common
{
    public interface IConfigurator<out TConfigurator>
    {
        TConfigurator WithGuards<T>() where T : IGuard;

        TConfigurator WithGuards<T1, T2>()
            where T1 : IGuard
            where T2 : IGuard;

        TConfigurator WithGuards<T1, T2, T3>()
            where T1 : IGuard
            where T2 : IGuard
            where T3 : IGuard;

        TConfigurator WithGuards<T1, T2, T3, T4>()
            where T1 : IGuard
            where T2 : IGuard
            where T3 : IGuard
            where T4 : IGuard;

        TConfigurator WithGuards<T1, T2, T3, T4, T5>()
            where T1 : IGuard
            where T2 : IGuard
            where T3 : IGuard
            where T4 : IGuard
            where T5 : IGuard;

        TConfigurator WithGuards(params Type[] guards);

        TConfigurator WithHooks<T>() where T : IHook;

        TConfigurator WithHooks<T1, T2>()
            where T1 : IHook
            where T2 : IHook;

        TConfigurator WithHooks<T1, T2, T3>()
            where T1 : IHook
            where T2 : IHook
            where T3 : IHook;

        TConfigurator WithHooks<T1, T2, T3, T4>()
            where T1 : IHook
            where T2 : IHook
            where T3 : IHook
            where T4 : IHook;

        TConfigurator WithHooks<T1, T2, T3, T4, T5>()
            where T1 : IHook
            where T2 : IHook
            where T3 : IHook
            where T4 : IHook
            where T5 : IHook;

        TConfigurator WithHooks(params Type[] hooks);
    }
}