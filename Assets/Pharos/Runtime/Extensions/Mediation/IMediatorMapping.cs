using System;
using System.Collections.Generic;

namespace Pharos.Extensions.Mediation
{
    public interface IMediatorMapping
    {
        Type ViewType { get; }

        Type MediatorType { get; }

        List<Type> GuardTypes { get; }

        List<Type> HookTypes { get; }

        bool AutoDestroyEnabled { get; }
    }
}