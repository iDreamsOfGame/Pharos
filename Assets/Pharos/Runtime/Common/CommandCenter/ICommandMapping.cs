using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pharos.Common.CommandCenter
{
    public interface ICommandMapping
    {
        Type CommandType { get; }
        
        MethodInfo ExecuteMethodInfo { get; }

        ParameterInfo[] ExecuteMethodParameters { get; }

        List<object> Guards { get; }

        List<object> Hooks { get; }

        bool ShouldExecuteOnce { get; set; }

        bool PayloadInjectionEnabled { get; set; }
        
        ICommandMapping AddGuards(params object[] guards);
        
        ICommandMapping AddGuards<T> ();
        
        ICommandMapping AddGuards<T1, T2> ();
        
        ICommandMapping AddGuards<T1, T2, T3> ();
        
        ICommandMapping AddGuards<T1, T2, T3, T4> ();
        
        ICommandMapping AddGuards<T1, T2, T3, T4, T5> ();
        
        ICommandMapping AddHooks(params object[] hooks);
        
        ICommandMapping AddHooks<T> ();
        
        ICommandMapping AddHooks<T1, T2> ();
        
        ICommandMapping AddHooks<T1, T2, T3> ();
        
        ICommandMapping AddHooks<T1, T2, T3, T4> ();
        
        ICommandMapping AddHooks<T1, T2, T3, T4, T5> ();
    }
}