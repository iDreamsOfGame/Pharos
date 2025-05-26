using System;
using NUnit.Framework;
using Pharos.Framework;
using Pharos.Framework.Helpers;
using PharosEditor.Tests.Framework.Supports;

namespace PharosEditor.Tests.Framework.Helpers
{
    [TestFixture]
    internal class ExtensionManagerTests
    {
        private ExtensionManager manager;

        [SetUp]
        public void Setup()
        {
            var context = new Context();
            manager = new ExtensionManager(context);
        }

        [TearDown]
        public void Cleanup()
        {
            CallbackExtension.StaticCallback = null;
            CallbackBundle.StaticCallback = null;
            CallbackExtensionInjectable.StaticCallback = null;
        }

        [Test]
        public void Install_ExtensionInstanceHasInstalled_ReturnsCorrectCallCount()
        {
            var callCount = 0;
            manager.Add(new CallbackExtension(delegate { callCount++; }));
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void Install_ExtensionTypeHasInstalled_ReturnsCorrectCallCount()
        {
            var callCount = 0;
            CallbackExtension.StaticCallback = delegate { callCount++; };
            manager.Add(typeof(CallbackExtension));
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void Install_ExtensionHasInstalledOnceForSameInstance_ReturnsCorrectCallCount()
        {
            var callCount = 0;
            var callback = (Action<IContext>)delegate { callCount++; };
            var extension = new CallbackExtension(callback);
            manager.Add(extension);
            manager.Add(extension);
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void Install_ExtensionHasInstalledOnceForSameType_ReturnsCorrectCallCount()
        {
            var callCount = 0;
            Action<IContext> callback = delegate { callCount++; };
            manager.Add(new CallbackExtension(callback));
            manager.Add(new CallbackExtension(callback));
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void UninstallAll_ExtensionHasUnplugged_ReturnsCorrectCallCount()
        {
            var callCount = 0;

            manager.Add(new CallbackExtension(null, Callback));
            manager.RemoveAll();
            Assert.That(callCount, Is.EqualTo(1));
            return;

            void Callback(IContext obj)
            {
                callCount++;
            }
        }
    }
}