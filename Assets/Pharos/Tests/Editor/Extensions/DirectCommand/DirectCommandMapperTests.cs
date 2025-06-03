using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Pharos.Common.CommandCenter;
using Pharos.Extensions.DirectCommand;
using PharosEditor.Tests.Common.CommandCenter.Supports;

namespace PharosEditor.Tests.Extensions.DirectCommand
{
    [TestFixture]
    internal class DirectCommandMapperTests
    {
        private Mock<ICommandMappingList> mockMappingList;

        private Mock<ICommandsExecutor> mockExecutor;

        private DirectCommandMapper subject;

        private ICommandMapping caughtMapping;

        [SetUp]
        public void Setup()
        {
            mockExecutor = new Mock<ICommandsExecutor>();
            mockMappingList = new Mock<ICommandMappingList>();
            mockMappingList.Setup(m => m.AddMapping(It.IsAny<ICommandMapping>()))
                .Callback<ICommandMapping>(r => caughtMapping = r);
        }

        [TearDown]
        public void Cleanup()
        {
            caughtMapping = null;
            subject = null;
        }

        [Test]
        public void Constructor_RegistersNewCommandMappingWithCommandMappingList_ReturnsInstanceIsExpectedType()
        {
            CreateMapper<NullCommand>();
            Assert.That(caughtMapping, Is.InstanceOf<ICommandMapping>());
        }

        [Test]
        public void Constructor_MapCreatesNewMapperInstance_ReturnsDifferentInstance()
        {
            var newMapper = CreateMapper<NullCommand>().Map<NullCommand2>();
            Assert.That(newMapper, Is.Not.Null);
            Assert.That(newMapper, Is.Not.EqualTo(subject));
        }

        [Test]
        public void Constructor_MappingIsExecuteOnceByDefault_ReturnsShouldExecuteOnceIsTrue()
        {
            CreateMapper<NullCommand>();
            Assert.That(caughtMapping.ShouldExecuteOnce, Is.True);
        }

        [Test]
        public void WithGuards_SetsGuardsOfMapping_ReturnsExpectedGuardsCollection()
        {
            var expected = new object[] { typeof(HappyGuard), typeof(GrumpyGuard) };
            CreateMapper<NullCommand>().WithGuards(expected);
            Assert.That(caughtMapping.Guards, Is.EqualTo(expected).AsCollection);
        }

        [Test]
        public void WithHooks_SetsHooksOfMapping_ReturnsExpectedHooksCollection()
        {
            var expected = new object[] { typeof(ClassReportingCallbackHook), typeof(ClassReportingCallbackHook) };
            CreateMapper<NullCommand>().WithHooks(expected);
            Assert.That(caughtMapping.Hooks, Is.EqualTo(expected).AsCollection);
        }

        [Test]
        public void WithPayloadInjection_SetsPayloadInjectionOfMapping_ReturnsPayloadInjectionIsFalse()
        {
            CreateMapper<NullCommand>().WithPayloadInjection(false);
            Assert.That(caughtMapping.PayloadInjectionEnabled, Is.False);
        }

        [Test]
        public void ExecuteCommands_CallsExecutorExecuteCommandsWithArguments_VerifiesMockObject()
        {
            var list = new List<ICommandMapping>();
            mockMappingList.Setup(m => m.Mappings).Returns(list);
            CreateMapper<NullCommand>().Execute();
            mockExecutor.Verify(e =>
                    e.ExecuteCommands(It.Is<List<ICommandMapping>>(arg1 => arg1 == list),
                        It.Is<CommandPayload>(arg2 => arg2.ValueTypeMap == null)),
                Times.Once);
        }

        private DirectCommandMapper CreateMapper<T>()
        {
            subject = new DirectCommandMapper(mockExecutor.Object, mockMappingList.Object, typeof(T));
            return subject;
        }
    }
}