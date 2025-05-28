using System;
using System.Collections.Generic;
using Pharos.Framework.Injection;

namespace Pharos.Framework.Helpers
{
    internal static class Hooks
    {
        private const string HookMethodName = nameof(IHook.Hook);
        
        public static void Hook(IInjector injector, IEnumerable<Action> hooks)
        {
            Hook(injector, hooks as IEnumerable<object>);
        }
        
        public static void Hook(IInjector injector, params Action[] hooks)
        {
            Hook(injector, hooks as IEnumerable<object>);
        }

        public static void Hook(params Action[] hooks)
        {
            Hook(null, hooks as IEnumerable<object>);
        }

        public static void Hook(IEnumerable<object> hooks)
        {
            Hook(null, hooks);
        }
        
        public static void Hook(params object[] hooks)
        {
            Hook(null, hooks);
        }
        
        public static void Hook(IInjector injector, params object[] hooks)
        {
            Hook(injector, hooks as IEnumerable<object>);
        }
        
        public static void Hook(IInjector injector, IEnumerable<object> hooks)
        {
            foreach (var hook in hooks)
            {
                if (hook is Action action)
                {
                    action.Invoke();
                    continue;
                }
                
                object hookInstance;
                if (hook is Type type)
                {
                    hookInstance = Actors.CreateInstance(type, injector);
                }
                else
                {
                    hookInstance = hook;
                }
                
                var hookMethod = hookInstance.GetType().GetMethod(HookMethodName);
                if (hookMethod == null)
                    throw new MissingMethodException(hookInstance.GetType().FullName, HookMethodName);
                
                hookMethod.Invoke(hookInstance, null);
            }
        }
    }
}