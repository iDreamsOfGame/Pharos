using System;

namespace Pharos.Common.CommandCenter
{
    public interface ICommandUnmapper
    {
        void FromCommand<T>();

        void FromCommand(Type commandType);

        void FromAll();
    }
}