using System;

namespace Pharos.Extensions.Mediation
{
    internal class NullMediatorUnmapper : IMediatorUnmapper
    {
        public void FromMediator<T>() where T : IMediator
        {
        }

        public void FromMediator(Type mediatorType)
        {
        }

        public void FromAll()
        {
        }
    }
}