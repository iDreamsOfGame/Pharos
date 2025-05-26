using System;
using Pharos.Framework;

namespace PharosEditor.Tests.Framework.Supports
{
    internal class CallbackExtensionInjectable : IExtension
    {
        public CallbackExtensionInjectable(Action<CallbackExtensionInjectable> callback = null)
        {
            Callback = callback ?? StaticCallback;
            StaticCallback = null;
        }

        public static Action<CallbackExtensionInjectable> StaticCallback { get; set; }

        public Action<CallbackExtensionInjectable> Callback { get; }

        public void Enable(IContext context)
        {
            Callback?.Invoke(this);
        }

        public void Disable(IContext context)
        {
        }
    }
}