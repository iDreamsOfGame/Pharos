using System;
using Pharos.Framework;

namespace PharosEditor.Tests.Framework.Supports
{
    internal class CallbackConfig : IConfig
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