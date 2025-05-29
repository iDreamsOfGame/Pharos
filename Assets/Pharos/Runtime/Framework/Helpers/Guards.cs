using System;
using System.Collections.Generic;
using Pharos.Framework.Injection;

namespace Pharos.Framework.Helpers
{
    internal static class Guards
    {
        private const string ApproveMethodName = nameof(IGuard.Approve);

        public static bool Approve(IInjector injector, IEnumerable<Func<bool>> guards)
        {
            return Approve(injector, guards as IEnumerable<object>);
        }

        public static bool Approve(IInjector injector, params Func<bool>[] guards)
        {
            return Approve(injector, guards as IEnumerable<object>);
        }

        public static bool Approve(params Func<bool>[] guards)
        {
            return Approve(null, guards as IEnumerable<object>);
        }

        public static bool Approve(IEnumerable<object> guards)
        {
            return Approve(null, guards);
        }

        public static bool Approve(params object[] guards)
        {
            return Approve(null, guards as IEnumerable<object>);
        }

        public static bool Approve(IInjector injector, params object[] guards)
        {
            return Approve(injector, guards as IEnumerable<object>);
        }

        public static bool Approve(IInjector injector, IEnumerable<object> guards)
        {
            foreach (var guard in guards)
            {
                if (guard is Func<bool> func)
                {
                    if (func.Invoke())
                        continue;

                    return false;
                }

                object guardInstance;
                if (guard is Type type)
                {
                    guardInstance = Actors.CreateInstance(type, injector);
                }
                else
                {
                    guardInstance = guard;
                }

                var approveMethod = guardInstance.GetType().GetMethod(ApproveMethodName);
                if (approveMethod == null)
                    throw new MissingMethodException(guardInstance.GetType().FullName, ApproveMethodName);

                if ((bool)approveMethod.Invoke(guardInstance, null) == false)
                    return false;
            }

            return true;
        }
    }
}