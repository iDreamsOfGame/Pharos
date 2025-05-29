using System;
using Pharos.Framework;
using UnityEngine;
using ILogHandler = Pharos.Framework.ILogHandler;

namespace Pharos.Extensions.DebugLogging
{
    public class DebugLogHandler : ILogHandler
    {
        private readonly IContext context;

        public DebugLogHandler(IContext context)
        {
            this.context = context;
        }

        public void Log(object source,
            LogLevel level,
            DateTime timestamp,
            object message,
            params object[] messageParameters)
        {
            var content = string.Format(timestamp.ToLongTimeString()
                                        + " "
                                        + level
                                        + " "
                                        + context
                                        + " "
                                        + source
                                        + " "
                                        + message,
                messageParameters);

            switch (level)
            {
                case >= LogLevel.Info:
                    Debug.Log(content);
                    break;

                case LogLevel.Warning:
                    Debug.LogWarning(content);
                    break;

                default:
                    Debug.LogError(content);
                    break;
            }
        }
    }
}