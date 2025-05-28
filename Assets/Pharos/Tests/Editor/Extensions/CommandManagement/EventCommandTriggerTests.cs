using System;
using Moq;
using NUnit.Framework;
using Pharos.Extensions.CommandManagement;
using Pharos.Extensions.EventManagement;
using Pharos.Framework.Injection;

namespace PharosEditor.Tests.Extensions.CommandManagement
{
    [TestFixture]
    internal class EventCommandTriggerTests
    {
        private Mock<IEventDispatcher> dispatcher;

        private EventCommandTrigger subject;

        [SetUp]
        public void Setup()
        {
            dispatcher = new Mock<IEventDispatcher>();
            subject = new EventCommandTrigger(new Injector(), dispatcher.Object, null);
        }

        [Test]
        public void Activate_ActivatingAddsListener_VerifiesMockObject()
        {
            subject.Activate();
            dispatcher.Verify(target => target.AddEventListener(It.IsAny<Enum>(), It.IsAny<Action<IEvent>>()), Times.Once);
        }

        [Test]
        public void Deactivate_DeactivatingRemovesListener_VerifiesMockObject()
        {
            subject.Deactivate();
            dispatcher.Verify(target => target.RemoveEventListener(It.IsAny<Enum>(), It.IsAny<Action<IEvent>>()), Times.Once);
        }
    }
}