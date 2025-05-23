using System;
using Pharos.Framework;

namespace PharosEditor.Tests.Supports
{
    public class CallbackConfig : IConfig
    {
        private readonly Action callback;

        public CallbackConfig(Action callback)
        {
            this.callback = callback;
        }

        public void Configure()
        {
            callback();
        }
    }
}