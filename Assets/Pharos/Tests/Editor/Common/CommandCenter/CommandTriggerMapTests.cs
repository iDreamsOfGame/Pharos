using Moq;
using NUnit.Framework;
using Pharos.Common.CommandCenter;
using PharosEditor.Tests.Common.CommandCenter.Supports;

namespace PharosEditor.Tests.Common.CommandCenter
{
    [TestFixture]
    internal class CommandTriggerMapTests
    {
        private Mock<CommandMapStub> host;

        private Mock<ICommandTrigger> trigger;

        private CommandMapStub stubby;

        [SetUp]
        public void Setup()
        {
            stubby = new CommandMapStub();
            host = new Mock<CommandMapStub>(MockBehavior.Strict);
            trigger = new Mock<ICommandTrigger>();
        }

        [Test]
        public void Constructor_KeyFactoryIsCalledWithParameters_VerifiesMockObject()
        {
            var args = new object[] { "hi", 5 };
            host.Setup(h => h.KeyFactory(args)).Returns("anyKey");
            new CommandTriggerMap(host.Object.KeyFactory, stubby.TriggerFactory).GetTrigger(args);
            host.Verify(h => h.KeyFactory(args), Times.Once);
        }

        [Test]
        public void Constructor_TriggerFactoryIsCalledWithParameters_VerifiesMockObject()
        {
            host.Setup(h => h.TriggerFactory("hi", 5)).Returns(trigger.Object);
            new CommandTriggerMap(stubby.KeyFactory, host.Object.TriggerFactory).GetTrigger("hi", 5);
            host.Verify(h => h.TriggerFactory("hi", 5), Times.Once);
        }

        [Test]
        public void GetTrigger_TriggerIsCachedByKey_ReturnsSameMappers()
        {
            var subject = new CommandTriggerMap(stubby.KeyFactory, stubby.TriggerFactory);
            object mapper1 = subject.GetTrigger("hi", 5);
            object mapper2 = subject.GetTrigger("hi", 5);
            Assert.That(mapper1, Is.Not.Null);
            Assert.That(mapper1, Is.EqualTo(mapper2));
        }

        [Test]
        public void RemoveTrigger_DeactivatesTriggers_VerifiesMockObject()
        {
            host.Setup(h => h.TriggerFactory(It.IsAny<object[]>())).Returns(trigger.Object);
            trigger.Setup(t => t.Deactivate());

            var subject = new CommandTriggerMap(stubby.KeyFactory, host.Object.TriggerFactory);
            subject.GetTrigger("hi", 5);
            subject.RemoveTrigger("hi", 5);
            trigger.Verify(t => t.Deactivate(), Times.Once);
        }
    }
}