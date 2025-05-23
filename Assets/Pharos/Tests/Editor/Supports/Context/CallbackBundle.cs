using System;
using Pharos.Framework;

namespace PharosEditor.Tests.Supports
{
    internal class CallbackBundle : IBundle
    {
        public CallbackBundle(Action<IContext> callback = null)
        {
            Callback = callback ?? StaticCallback;
            StaticCallback = null;
        }

        public static Action<IContext> StaticCallback { get; set; }

        public Action<IContext> Callback { get; }

        public void Enable(IContext context)
        {
            Callback?.Invoke(context);
        }

        public void Disable(IContext context)
        {
        }
    }
}