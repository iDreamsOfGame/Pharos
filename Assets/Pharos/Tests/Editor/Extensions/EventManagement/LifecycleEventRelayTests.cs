using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Common.EventCenter;
using Pharos.Extensions.EventManagement;
using Pharos.Framework;

namespace PharosEditor.Tests.Extensions.EventManagement
{
    [TestFixture]
    internal class LifecycleEventRelayTests
    {
        private static readonly List<LifecycleEvent.Type> LifecycleEventTypes = new()
        {
            LifecycleEvent.Type.Initializing, LifecycleEvent.Type.Initialized,
            LifecycleEvent.Type.Suspending, LifecycleEvent.Type.Suspended,
            LifecycleEvent.Type.Resuming, LifecycleEvent.Type.Resumed,
            LifecycleEvent.Type.Destroying, LifecycleEvent.Type.Destroyed
        };

        private IContext context;

        private LifecycleEventRelay subject;

        private List<object> reportedTypes;

        [SetUp]
        public void Setup()
        {
            context = new Context();
            EventDispatcher.Instance = new EventDispatcher();
            subject = new LifecycleEventRelay(context);
            reportedTypes = new List<object>();
        }

        [TearDown]
        public void Cleanup()
        {
            if (context.HasInitialized && !context.HasDestroyed)
                context.Destroy();
        }

        [Test]
        public void StateChangedEventIsRelayed_ReturnsCollectionContainsExpectedEventType()
        {
            ListenFor(new List<LifecycleEvent.Type> { LifecycleEvent.Type.StateChanged });
            context.Initialize();
            Assert.That(reportedTypes, Contains.Item(LifecycleEvent.Type.StateChanged));
        }

        [Test]
        public void LifecycleEventsAreRelayed_ReturnsExpectedEventTypeList()
        {
            ListenFor(LifecycleEventTypes);
            context.Initialize();
            context.Suspend();
            context.Resume();
            context.Destroy();
            Assert.That(reportedTypes, Is.EquivalentTo(LifecycleEventTypes));
        }

        [Test]
        public void LifecycleEventsAreNotRelayedAfterDestroy_ReturnsEmptyEventTypeList()
        {
            ListenFor(LifecycleEventTypes);
            subject.Destroy();
            context.Initialize();
            context.Suspend();
            context.Resume();
            context.Destroy();
            Assert.That(reportedTypes, Is.Empty);
        }

        private void ListenFor(List<LifecycleEvent.Type> types)
        {
            foreach (var type in types)
            {
                EventDispatcher.Instance.AddEventListener(type, CatchEvent);
            }
        }

        private void CatchEvent(IEvent e)
        {
            reportedTypes.Add(e.EventType);
        }
    }
}