using System;
using JetBrains.Annotations;
using Pharos.Framework;

namespace PharosEditor.Tests.Framework.Supports
{
    internal class CallbackExtension : IExtension
    {
        [UsedImplicitly]
        public CallbackExtension()
        {
            ExtendCallback = StaticCallback;
        }

        public CallbackExtension(Action<IContext> extendCallback = null, Action<IContext> unplugCallback = null)
        {
            ExtendCallback = extendCallback ?? StaticCallback;
            UnplugCallback = unplugCallback ?? StaticCallback;
            StaticCallback = null;
        }

        public static Action<IContext> StaticCallback { get; set; }

        public Action<IContext> ExtendCallback { get; }

        public Action<IContext> UnplugCallback { get; }

        public void Enable(IContext context)
        {
            ExtendCallback?.Invoke(context);
        }

        public void Disable(IContext context)
        {
            UnplugCallback?.Invoke(context);
        }
    }
}