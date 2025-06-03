using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Pharos.Common.CommandCenter;
using Pharos.Extensions.DirectAsyncCommand;
using PharosEditor.Tests.Common.CommandCenter.Supports;
using PharosEditor.Tests.Extensions.DirectAsyncCommand.Supports;

namespace PharosEditor.Tests.Extensions.DirectAsyncCommand
{
    [TestFixture]
    internal class DirectAsyncCommandMapperTests
    {
        private ICommandMapping caughtMapping;

        private Mock<IAsyncCommandsExecutor> mockExecutor;

        private Mock<ICommandMappingList> mockMappingList;

        private DirectAsyncCommandMapper subject;

        [SetUp]
        public void Setup()
        {
            mockExecutor = new Mock<IAsyncCommandsExecutor>();
            mockMappingList = new Mock<ICommandMappingList>();
            mockMappingList.Setup(m => m.AddMapping(It.IsAny<ICommandMapping>())).Callback<ICommandMapping>(r => caughtMapping = r);
        }

        [TearDown]
        public void Cleanup()
        {
            caughtMapping = null;
            subject = null;
        }
        
        [Test]
        public void Constructor_MappingExecuteOnceByDefault_ReturnsTrue()
        {
            CreateMapper<NullAsyncCommand>();
            Assert.That(caughtMapping.ShouldExecuteOnce, Is.True);
        }

        [Test]
        public void Constructor_RegistersNewCommandMappingWithCommandMappingList_ReturnsInstanceOfExpectedType()
        {
            CreateMapper<NullAsyncCommand>();
            Assert.That(caughtMapping, Is.InstanceOf<ICommandMapping>());
        }

        [Test]
        public void Map_CreatesNewMapperInstance_ReturnsNewInstance()
        {
            var newMapper = CreateMapper<NullAsyncCommand>().Map<NullAsyncCommand2>();
            Assert.That(newMapper, Is.Not.Null);
            Assert.That(newMapper, Is.Not.EqualTo(subject));
        }

        [Test]
        public void WithGuards_SetsGuardsOfMapping_ReturnsExpectedCollection()
        {
            var expected = new object[] { typeof(HappyGuard), typeof(GrumpyGuard) };
            CreateMapper<NullAsyncCommand>().WithGuards(expected);
            Assert.That(caughtMapping.Guards, Is.EqualTo(expected).AsCollection);
        }
        
        [Test]
        public void WithHooks_SetsHooksOfMapping_ReturnsExpectedCollection()
        { 
            var expected = new object[] { typeof(ClassReportingCallbackHook), typeof(ClassReportingCallbackHook) };
            CreateMapper<NullAsyncCommand>().WithHooks(expected);
            Assert.That(caughtMapping.Hooks, Is.EqualTo(expected).AsCollection);
        }
        
        [Test]
        public void WithPayloadInjection_SetsPayloadInjectionOfMapping_ReturnsFalse()
        {
            CreateMapper<NullAsyncCommand>().WithPayloadInjection(false);
            Assert.That(caughtMapping.PayloadInjectionEnabled, Is.False);
        }

        [Test]
        public void Execute_CallsExecutorExecuteMethodWithArguments_VerifiesMockObject()
        {
            var mappings = new List<ICommandMapping>();
            mockMappingList.Setup(m => m.Mappings).Returns(mappings);
            CreateMapper<NullAsyncCommand>().Execute();

            mockExecutor.Verify(e =>
                    e.ExecuteCommands(It.Is<List<ICommandMapping>>(arg1 => arg1 == mappings),
                        It.Is<CommandPayload>(arg2 => true)),
                Times.Once);
        }

        private DirectAsyncCommandMapper CreateMapper<T>()
        {
            subject = new DirectAsyncCommandMapper(mockExecutor.Object, mockMappingList.Object, typeof(T));
            return subject;
        }
    }
}