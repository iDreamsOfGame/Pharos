using System;
using System.Collections.Generic;
using Pharos.Framework.Injection;

namespace Pharos.Framework.Helpers
{
    internal static class Hooks
    {
        public static void Hook(IEnumerable<Type> hooks)
        {
            Hook(null, hooks);
        }

        public static void Hook(params Type[] hooks)
        {
            Hook(null, hooks);
        }

        public static void Hook(IInjector injector, params Type[] hooks)
        {
            Hook(injector, hooks as IEnumerable<Type>);
        }

        public static void Hook(IInjector injector, IEnumerable<Type> hookTypes)
        {
            foreach (var hookType in hookTypes)
            {
                if (InstanceActivator.CreateInstance(hookType, injector) is not IHook hook)
                    continue;

                hook.Hook();
            }
        }
    }
}