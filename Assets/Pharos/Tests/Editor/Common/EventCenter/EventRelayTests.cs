using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Common.EventCenter;
using PharosEditor.Tests.Common.EventCenter.Supports;

namespace PharosEditor.Tests.Common.EventCenter
{
    [TestFixture]
    internal class EventRelayTests
    {
        private IEventDispatcher source;

        private IEventDispatcher destination;

        private EventRelay subject;

        private List<Enum> reportedTypes;

        [SetUp]
        public void Setup()
        {
            source = new EventDispatcher();
            destination = new EventDispatcher();
            reportedTypes = new List<Enum>();
        }

        [Test]
        public void Constructor_NoRelayBeforeStart_ReturnsEmptyEventTypeList()
        {
            CreateRelayFor(SupportEvent.Type.Type1);
            source.Dispatch(new SupportEvent(SupportEvent.Type.Type2));
            Assert.That(reportedTypes, Is.Empty);
        }

        [Test]
        public void Start_RelaysSpecifiedEvents_ReturnsExpectedEventTypeList()
        {
            CreateRelayFor(SupportEvent.Type.Type1, SupportEvent.Type.Type2).Start();
            source.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            source.Dispatch(new SupportEvent(SupportEvent.Type.Type2));
            Assert.That(reportedTypes, Is.EquivalentTo(new List<Enum> { SupportEvent.Type.Type1, SupportEvent.Type.Type2 }));
        }

        [Test]
        public void Start_IgnoresUnspecifiedEvents_ReturnsEmptyEventTypeList()
        {
            CreateRelayFor().Start();
            source.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            Assert.That(reportedTypes, Is.Empty);
        }

        [Test]
        public void Start_RelaysSpecifiedEventsButIgnoresUnspecifiedEvents_ReturnsExpectedEventTypeList()
        {
            CreateRelayFor(SupportEvent.Type.Type1).Start();
            source.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            source.Dispatch(new SupportEvent(SupportEvent.Type.Type2));
            Assert.That(reportedTypes, Is.EquivalentTo(new List<Enum> { SupportEvent.Type.Type1 }));
        }

        [Test]
        public void Stop_NoRelayAfterStop_ReturnsEmptyEmptyEventTypeList()
        {
            CreateRelayFor(SupportEvent.Type.Type1).Start().Stop();
            source.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            Assert.That(reportedTypes, Is.Empty);
        }

        [Test]
        public void Start_RelayResumes_ReturnsExpectedEventTypeList()
        {
            CreateRelayFor(SupportEvent.Type.Type1).Start().Stop().Start();
            source.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            Assert.That(reportedTypes, Is.EquivalentTo(new List<Enum>() { SupportEvent.Type.Type1 }));
        }

        private EventRelay CreateRelayFor(params Enum[] types)
        {
            subject = new EventRelay(source, destination, new List<Enum>(types));
            foreach (var type in types)
            {
                destination.AddEventListener(type, CatchEvent);
            }

            return subject;
        }

        private void CatchEvent(IEvent e)
        {
            reportedTypes.Add(e.EventType);
        }
    }
}