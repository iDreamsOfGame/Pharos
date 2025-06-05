using System;
using System.Collections.Generic;
using Pharos.Framework.Injection;

namespace Pharos.Framework.Helpers
{
    internal static class Guards
    {
        public static bool Approve(IEnumerable<Type> guardTypes)
        {
            return Approve(null, guardTypes);
        }

        public static bool Approve(params Type[] guardTypes)
        {
            return Approve(null, guardTypes as IEnumerable<Type>);
        }

        public static bool Approve(IInjector injector, params Type[] guardTypes)
        {
            return Approve(injector, guardTypes as IEnumerable<Type>);
        }

        public static bool Approve(IInjector injector, IEnumerable<Type> guardTypes)
        {
            foreach (var guardType in guardTypes)
            {
                if (InstanceActivator.CreateInstance(guardType, injector) is not IGuard guard)
                    return false;

                if (!guard.Approve())
                    return false;
            }

            return true;
        }
    }
}