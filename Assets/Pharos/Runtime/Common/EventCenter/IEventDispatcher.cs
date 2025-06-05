using System;

namespace Pharos.Common.EventCenter
{
    public interface IEventDispatcher
    {
        void AddEventListener<T>(Enum type, Action<T> listener);

        void AddEventListener(Enum type, Action<IEvent> listener);

        void AddEventListener(Enum type, Action listener);

        void AddEventListener(Enum type, Delegate listener);

        void RemoveEventListener<T>(Enum type, Action<T> listener);

        void RemoveEventListener(Enum type, Action<IEvent> listener);

        void RemoveEventListener(Enum type, Action listener);

        void RemoveEventListener(Enum type, Delegate listener);

        void RemoveEventListeners(Enum type);

        void RemoveEventListeners(object target);

        void RemoveAllEventListeners();

        bool HasEventListener(Enum type);

        void Dispatch(IEvent e);
    }
}