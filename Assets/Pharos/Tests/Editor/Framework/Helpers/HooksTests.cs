using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Framework;
using Pharos.Framework.Helpers;
using Pharos.Framework.Injection;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Framework.Helpers
{
    [TestFixture]
    internal class HooksTests
    {
        private class CallbackHook : IHook
        {
            [Inject(true, "hookCallback")]
            public Action Callback { get; internal set; }

            public void Hook()
            {
                Callback?.Invoke();
            }
        }

        private IInjector injector;

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
        public void Hook_ActionHook_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            Hooks.Hook(delegate { callCount++; });
            Assert.AreEqual(callCount, 1);
        }

        [Test]
        public void Hook_ActionHookList_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            var hook = (Action)delegate { callCount++; };
            Hooks.Hook(new List<Action> { hook, hook });
            Assert.AreEqual(callCount, 2);
        }

        [Test]
        public void Hook_TypeHook_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            injector.Map<Action>("hookCallback").ToValue((Action)delegate { callCount++; });
            Hooks.Hook(injector, typeof(CallbackHook));
            Assert.AreEqual(callCount, 1);
        }

        [Test]
        public void Hook_HookInstance_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            var hook = new CallbackHook
            {
                Callback = delegate { callCount++; }
            };
            Hooks.Hook(hook);
            Assert.AreEqual(callCount, 1);
        }

        [Test]
        public void Hook_InstanceWithoutHookMethod_ThrowsException()
        {
            Assert.Throws<MissingMethodException>(() =>
            {
                var invalidHook = new object();
                Hooks.Hook(invalidHook);
            });
        }
    }
}