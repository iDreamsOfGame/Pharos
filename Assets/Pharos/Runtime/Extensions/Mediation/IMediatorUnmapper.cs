using System;

namespace Pharos.Extensions.Mediation
{
    public interface IMediatorUnmapper
    {
        void FromMediator<T>() where T : IMediator;

        void FromMediator(Type mediatorType);

        void FromAll();
    }
}