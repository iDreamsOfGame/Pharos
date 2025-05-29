using System;
using Pharos.Common.CommandCenter;

namespace Pharos.Extensions.DirectCommand
{
    public interface IDirectCommandMap : IDirectCommandMapper
    {
        void Detain(object command);

        void Release(object command);

        IDirectCommandMap AddMappingProcessor(Action<ICommandMapping> processor);
    }
}