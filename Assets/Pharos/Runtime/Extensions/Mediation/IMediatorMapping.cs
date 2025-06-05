using System;
using System.Collections.Generic;

namespace Pharos.Extensions.Mediation
{
    public interface IMediatorMapping
    {
        Type ViewType { get; }

        Type MediatorType { get; }

        List<object> Guards { get; }

        List<object> Hooks { get; }

        bool AutoDestroyEnabled { get; }
    }
}