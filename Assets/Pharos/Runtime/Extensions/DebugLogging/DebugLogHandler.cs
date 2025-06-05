using System;
using System.Text;
using Pharos.Framework;
using UnityEngine;
using ILogHandler = Pharos.Framework.ILogHandler;

namespace Pharos.Extensions.DebugLogging
{
    public class DebugLogHandler : ILogHandler
    {
        private const char BlankChar = ' ';
        
        private readonly IContext context;

        private readonly StringBuilder contentBuilder;

        public DebugLogHandler(IContext context)
        {
            this.context = context;
            contentBuilder = new StringBuilder();
        }

        public void Log(object source,
            LogLevel level,
            DateTime timestamp,
            object message,
            params object[] messageParameters)
        {
            contentBuilder.Append(timestamp.ToLongTimeString());
            contentBuilder.Append(BlankChar);
            contentBuilder.Append(level);
            contentBuilder.Append(BlankChar);
            contentBuilder.Append(context);
            contentBuilder.Append(BlankChar);
            contentBuilder.Append(source);
            contentBuilder.Append(BlankChar);
            contentBuilder.AppendFormat(message.ToString(), messageParameters);
            var content = contentBuilder.ToString();

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

            contentBuilder.Clear();
        }
    }
}