using System;

namespace Pharos.Extensions.DirectCommand
{
    public interface IDirectCommandConfigurator : IDirectCommandMapper
    {
        IDirectCommandConfigurator WithGuards(params Type[] guards);

        IDirectCommandConfigurator WithHooks(params Type[] hooks);

        IDirectCommandConfigurator WithPayloadInjection(bool value = true);
    }
}