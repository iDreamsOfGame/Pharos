using System;
using System.Collections.Generic;

namespace Pharos.Extensions.Mediation
{
    public class MediatorMapping : IEquatable<MediatorMapping>, IMediatorMapping, IMediatorConfigurator
    {
        public Type ViewType { get; }

        public Type MediatorType { get; }

        public List<object> Guards { get; private set; }

        public List<object> Hooks { get; private set; }

        public bool AutoDestroyEnabled { get; private set; } = true;

        public MediatorMapping(Type viewType, Type mediatorType)
        {
            ViewType = viewType;
            MediatorType = mediatorType;
        }

        public IMediatorConfigurator WithGuards<T>()
        {
            return WithGuards(typeof(T));
        }

        public IMediatorConfigurator WithGuards<T1, T2>()
        {
            return WithGuards(typeof(T1), typeof(T2));
        }

        public IMediatorConfigurator WithGuards<T1, T2, T3>()
        {
            return WithGuards(typeof(T1), typeof(T2), typeof(T3));
        }

        public IMediatorConfigurator WithGuards<T1, T2, T3, T4>()
        {
            return WithGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public IMediatorConfigurator WithGuards<T1, T2, T3, T4, T5>()
        {
            return WithGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        public IMediatorConfigurator WithGuards(params object[] guards)
        {
            if (guards is { Length: > 0 })
            {
                Guards ??= new List<object>();
                Guards.AddRange(guards);
            }
            
            return this;
        }

        public IMediatorConfigurator WithHooks<T>()
        {
            return WithHooks(typeof(T));
        }

        public IMediatorConfigurator WithHooks<T1, T2>()
        {
            return WithHooks(typeof(T1), typeof(T2));
        }

        public IMediatorConfigurator WithHooks<T1, T2, T3>()
        {
            return WithHooks(typeof(T1), typeof(T2), typeof(T3));
        }

        public IMediatorConfigurator WithHooks<T1, T2, T3, T4>()
        {
            return WithHooks(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public IMediatorConfigurator WithHooks<T1, T2, T3, T4, T5>()
        {
            return WithHooks(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        public IMediatorConfigurator WithHooks(params object[] hooks)
        {
            if (hooks is { Length: > 0 })
            {
                Hooks ??= new List<object>();
                Hooks.AddRange(hooks);
            }
            
            return this;
        }

        public IMediatorConfigurator EnableAutoDestroy()
        {
            AutoDestroyEnabled = true;
            return this;
        }

        public IMediatorConfigurator DisableAutoDestroy()
        {
            AutoDestroyEnabled = false;
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