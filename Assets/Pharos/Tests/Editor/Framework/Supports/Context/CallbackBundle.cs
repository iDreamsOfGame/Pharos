using System;
using Pharos.Framework;

namespace PharosEditor.Tests.Framework.Supports
{
    internal class CallbackBundle : IExtension
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