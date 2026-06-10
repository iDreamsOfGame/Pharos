using System;
using Pharos.Common.CommandCenter;
using UnityEngine.Scripting;

namespace Pharos.Extensions.DirectCommand
{
    [RequireImplementors]
    public interface IDirectCommandMap : IDirectCommandMapper
    {
        void Detain(object command);

        void Release(object command);

        IDirectCommandMap AddMappingProcessor(Action<ICommandMapping> processor);
    }
}