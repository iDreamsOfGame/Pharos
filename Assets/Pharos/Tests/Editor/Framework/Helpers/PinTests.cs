using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Framework.Helpers;

namespace PharosEditor.Tests.Framework.Helpers
{
    [TestFixture]
    internal class PinTests
    {
        private Pin pin;

        private object instance;

        [SetUp]
        public void Setup()
        {
            instance = new object();
            pin = new Pin();
        }

        [Test]
        public void Detain_DispatchEvent_ReturnsCorrectEventCount()
        {
            var eventCount = 0;
            pin.Detained += delegate { eventCount++; };
            pin.Detain(instance);
            Assert.That(eventCount, Is.EqualTo(1));
        }

        [Test]
        public void Detain_DispatchEventOncePerValidDetainment_ReturnsCorrectEventCount()
        {
            var eventCount = 0;
            pin.Detained += delegate { eventCount++; };
            pin.Detain(instance);
            pin.Detain(instance);
            Assert.That(eventCount, Is.EqualTo(1));
        }

        [Test]
        public void Release_DispatchEvent_ReturnsCorrectEventCount()
        {
            var eventCount = 0;
            pin.Released += delegate { eventCount++; };
            pin.Detain(instance);
            pin.Release(instance);
            Assert.That(eventCount, Is.EqualTo(1));
        }

        [Test]
        public void Release_DispatchEventOncePerValidRelease_ReturnsCorrectEventCount()
        {
            var eventCount = 0;
            pin.Released += delegate { eventCount++; };
            pin.Detain(instance);
            pin.Release(instance);
            pin.Release(instance);
            Assert.That(eventCount, Is.EqualTo(1));
        }

        [Test]
        public void Release_DoesNotDispatchEventIfInstanceWasNotDetained_ReturnsCorrectEventCount()
        {
            var eventCount = 0;
            pin.Released += delegate { eventCount++; };
            pin.Release(instance);
            Assert.That(eventCount, Is.EqualTo(0));
        }

        [Test]
        public void ReleaseAll_DispatchEventsForAllInstances_ReturnsCorrectReleasedObjects()
        {
            var releasedObjects = new List<object>();
            pin.Released += delegate(object obj) { releasedObjects.Add(obj); };
            var instanceA = new object();
            var instanceB = new object();
            var instanceC = new object();
            pin.Detain(instanceA);
            pin.Detain(instanceB);
            pin.Detain(instanceC);
            pin.ReleaseAll();
            var instanceAbc = new[] { instanceA, instanceB, instanceC };
            Assert.That(releasedObjects.ToArray(), Is.EqualTo(instanceAbc).AsCollection);
        }
    }
}