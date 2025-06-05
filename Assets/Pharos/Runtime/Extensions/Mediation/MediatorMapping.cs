using System;
using System.Collections.Generic;
using Pharos.Framework;

namespace Pharos.Extensions.Mediation
{
    public class MediatorMapping : IEquatable<MediatorMapping>, IMediatorMapping, IMediatorConfigurator
    {
        public Type ViewType { get; }

        public Type MediatorType { get; }

        public List<Type> GuardTypes { get; private set; }

        public List<Type> HookTypes { get; private set; }

        public MediatorMapping(Type viewType, Type mediatorType)
        {
            ViewType = viewType;
            MediatorType = mediatorType;
        }

        public IMediatorConfigurator WithGuards<T>() where T : IGuard
        {
            return WithGuards(typeof(T));
        }

        public IMediatorConfigurator WithGuards<T1, T2>()
            where T1 : IGuard
            where T2 : IGuard
        {
            return WithGuards(typeof(T1), typeof(T2));
        }

        public IMediatorConfigurator WithGuards<T1, T2, T3>()
            where T1 : IGuard
            where T2 : IGuard
            where T3 : IGuard
        {
            return WithGuards(typeof(T1), typeof(T2), typeof(T3));
        }

        public IMediatorConfigurator WithGuards<T1, T2, T3, T4>()
            where T1 : IGuard
            where T2 : IGuard
            where T3 : IGuard
            where T4 : IGuard
        {
            return WithGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public IMediatorConfigurator WithGuards<T1, T2, T3, T4, T5>()
            where T1 : IGuard
            where T2 : IGuard
            where T3 : IGuard
            where T4 : IGuard
            where T5 : IGuard
        {
            return WithGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        public IMediatorConfigurator WithGuards(params Type[] guards)
        {
            if (guards is { Length: > 0 })
            {
                GuardTypes ??= new List<Type>();
                GuardTypes.AddRange(guards);
            }

            return this;
        }

        public IMediatorConfigurator WithHooks<T>() where T : IHook
        {
            return WithHooks(typeof(T));
        }

        public IMediatorConfigurator WithHooks<T1, T2>()
            where T1 : IHook
            where T2 : IHook
        {
            return WithHooks(typeof(T1), typeof(T2));
        }

        public IMediatorConfigurator WithHooks<T1, T2, T3>()
            where T1 : IHook
            where T2 : IHook
            where T3 : IHook
        {
            return WithHooks(typeof(T1), typeof(T2), typeof(T3));
        }

        public IMediatorConfigurator WithHooks<T1, T2, T3, T4>()
            where T1 : IHook
            where T2 : IHook
            where T3 : IHook
            where T4 : IHook
        {
            return WithHooks(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public IMediatorConfigurator WithHooks<T1, T2, T3, T4, T5>()
            where T1 : IHook
            where T2 : IHook
            where T3 : IHook
            where T4 : IHook
            where T5 : IHook
        {
            return WithHooks(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        public IMediatorConfigurator WithHooks(params Type[] hooks)
        {
            if (hooks is { Length: > 0 })
            {
                HookTypes ??= new List<Type>();
                HookTypes.AddRange(hooks);
            }

            return this;
        }

        public bool Equals(MediatorMapping other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return ViewType == other.ViewType && MediatorType == other.MediatorType;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((MediatorMapping)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ViewType, MediatorType);
        }
    }
}