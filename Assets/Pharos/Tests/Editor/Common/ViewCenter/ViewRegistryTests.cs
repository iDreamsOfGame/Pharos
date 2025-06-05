using System;
using NUnit.Framework;
using Pharos.Common.ViewCenter;

namespace PharosEditor.Tests.Common.ViewCenter
{
    [TestFixture]
    internal class ViewRegistryTests
    {
        private class FooView : IView
        {
        }

        private class CallbackViewHandler : IViewHandler
        {
            private readonly Action viewInitializedCallback;

            private readonly Action viewDestroyingCallback;

            public CallbackViewHandler(Action viewInitializedCallback = null, Action viewDestroyingCallback = null)
            {
                this.viewInitializedCallback = viewInitializedCallback;
                this.viewDestroyingCallback = viewDestroyingCallback;
            }

            public void HandleViewInitialized(IView view, Type viewType)
            {
                viewInitializedCallback?.Invoke();
            }

            public void HandleViewDestroying(IView view)
            {
                viewDestroyingCallback?.Invoke();
            }
        }

        [Test]
        public void RegisterView_HandleViewInitializedMethodOfViewHandlerIsCalled_ReturnsTrue()
        {
            var hasViewInitialized = false;
            var viewHandler = new CallbackViewHandler(delegate { hasViewInitialized = true; });
            ViewRegistry.Instance.AddHandler(viewHandler);
            var view = new FooView();
            ViewRegistry.Instance.RegisterView(view);
            Assert.That(hasViewInitialized, Is.True);
        }

        [Test]
        public void RemoveView_HandleViewDestroyingMethodOfViewHandlerIsCalled_ReturnsTrue()
        {
            var hasViewDestroying = false;
            var viewHandler = new CallbackViewHandler(null, delegate { hasViewDestroying = true; });
            ViewRegistry.Instance.AddHandler(viewHandler);
            var view = new FooView();
            ViewRegistry.Instance.RemoveView(view);
            Assert.That(hasViewDestroying, Is.True);
        }
    }
}