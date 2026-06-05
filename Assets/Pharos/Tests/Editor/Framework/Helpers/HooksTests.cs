using System;
using NUnit.Framework;
using Pharos.Framework;
using Pharos.Framework.Helpers;
using Pharos.Framework.Injection;
using VContainer;

namespace PharosEditor.Tests.Framework.Helpers
{
    [TestFixture]
    internal class HooksTests
    {
        [InjectIgnore]
        private class CallbackHook : IHook
        {
            [Inject, Key("hookCallback")]
            public Action Callback { get; internal set; }

            public void Hook()
            {
                Callback?.Invoke();
            }
        }

        private IPharosInjector injector;

        [SetUp]
        public void Setup()
        {
            injector = new Injector();
        }

        [TearDown]
        public void Cleanup()
        {
            injector.Dispose();
            injector = null;
        }

        [Test]
        public void Hook_TypeHook_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            injector.Map<Action>("hookCallback").ToValue((Action)delegate { callCount++; });
            Hooks.Hook(injector, typeof(CallbackHook));
            Assert.AreEqual(callCount, 1);
        }
    }
}