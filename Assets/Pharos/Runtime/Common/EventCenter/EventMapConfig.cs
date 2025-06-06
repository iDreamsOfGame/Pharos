using System;

namespace Pharos.Common.EventCenter
{
    public readonly struct EventMapConfig : IEquatable<EventMapConfig>
    {
        public EventMapConfig(Delegate listener)
        {
            Listener = listener;
            IsAction = listener is Action;
            IsActionWithParameter = listener is Action<IEvent>;

            if (!IsAction && !IsActionWithParameter)
            {
                HasParameter = listener.Method.GetParameters().Length > 0;
            }
            else
            {
                HasParameter = false;
            }
        }

        public Delegate Listener { get; }

        public bool IsAction { get; }

        public bool IsActionWithParameter { get; }

        public bool HasParameter { get; }

        public bool Equals(EventMapConfig other) => ReferenceEquals(Listener, other.Listener) || Listener.Equals(other.Listener);

        public override bool Equals(object obj) => obj != null && Listener.Equals((obj is EventMapConfig data ? data : default).Listener);

        public override int GetHashCode() => Listener?.GetHashCode() ?? 0;

        public void Invoke(IEvent e)
        {
            if (IsAction)
            {
                Invoke(Listener as Action);
            }
            else if (IsActionWithParameter)
            {
                ((Action<IEvent>)Listener).Invoke(e);
            }
            else
            {
                DynamicInvoke(e);
            }
        }

        private static void Invoke(Action action)
        {
            action?.Invoke();
        }

        private void DynamicInvoke(IEvent e)
        {
            if (HasParameter)
            {
                Listener.DynamicInvoke(e);
            }
            else
            {
                Listener.DynamicInvoke();
            }
        }
    }
}