using System;
using Moq;
using NUnit.Framework;
using Pharos.Common.CommandCenter;

namespace PharosEditor.Tests.Common.CommandCenter
{
    [TestFixture]
    internal class CommandMapperTests
    {
        private Mock<ICommandMappingList> mappings;

        private CommandMapper subject;

        [SetUp]
        public void Setup()
        {
            mappings = new Mock<ICommandMappingList>();
            subject = new CommandMapper(mappings.Object);
        }

        [Test]
        public void ToCommand_CreatesICommandConfigurator_ReturnsNotNullInstanceOfExpectedType()
        {
            Assert.That(subject, Is.Not.Null);
            Assert.That(subject.ToCommand<string>(), Is.InstanceOf<ICommandConfigurator>());
        }

        [Test]
        public void ToCommand_PassesCommandMappingToMappingList_VerifiesMockObject()
        {
            subject.ToCommand(typeof(string));
            mappings.Verify(m => m.AddMapping(It.IsAny<ICommandMapping>()), Times.Once);
        }

        [Test]
        public void FromCommand_DelegatesToMappingList_VerifiesMockObject()
        {
            var type = typeof(string);
            subject.FromCommand(type);
            mappings.Verify(m => m.RemoveMappingFor(It.Is<Type>(arg => type == arg)), Times.Once);
        }

        [Test]
        public void FromAll_DelegatesToMappingList_VerifiesMockObject()
        {
            subject.FromAll();
            mappings.Verify(m => m.RemoveAllMappings(), Times.Once);
        }
    }
}