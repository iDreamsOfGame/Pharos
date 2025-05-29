using System;
using System.Reflection;

namespace Pharos.Common.CommandCenter
{
    internal readonly struct CommandExecuteMethodInfo
    {
        public CommandExecuteMethodInfo(Type commandType)
        {
            try
            {
                MethodInfo = commandType.GetMethod(nameof(ICommand.Execute));
                MethodParameters = MethodInfo?.GetParameters() ?? Array.Empty<ParameterInfo>();
            }
            catch (Exception)
            {
                // ignored
                MethodInfo = null;
                MethodParameters = Array.Empty<ParameterInfo>();
            }
        }

        public MethodInfo MethodInfo { get; }

        public ParameterInfo[] MethodParameters { get; }
    }
}