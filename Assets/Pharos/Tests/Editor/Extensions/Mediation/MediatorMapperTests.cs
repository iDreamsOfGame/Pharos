using System;
using Moq;
using NUnit.Framework;
using Pharos.Extensions.Mediation;
using Pharos.Framework;
using PharosEditor.Tests.Extensions.Mediation.Supports;

namespace PharosEditor.Tests.Extensions.Mediation
{
    [TestFixture]
    internal class MediatorMapperTests
    {
        private Mock<IMediatorViewHandler> handler;

        private Mock<ILogger> logger;

        private MediatorMapper mapper;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger>();
            handler = new Mock<IMediatorViewHandler>();
            mapper = new MediatorMapper(typeof(SupportView), handler.Object, logger.Object);
        }

        [Test]
        public void ToMediator_RegistersMappingWithHandler_VerifiesMockObject()
        {
            var configurator = mapper.ToMediator<NullMediator>();
            handler.Verify(h => h.AddMapping(It.Is<IMediatorMapping>(arg => arg == configurator)), Times.Once);
        }

        [Test]
        public void ToMediator_UnregistersOldMappingAndRegistersNewOneWhenOverwritten_VerifiesMockObject()
        {
            var oldConfigurator = mapper.ToMediator<NullMediator>();
            var newConfigurator = mapper.ToMediator<NullMediator>();
            handler.Verify(h => h.RemoveMapping(It.Is<IMediatorMapping>(arg => arg == oldConfigurator)), Times.Once);
            handler.Verify(h => h.AddMapping(It.Is<IMediatorMapping>(arg => arg == newConfigurator)), Times.Once);
        }

        [Test]
        public void ToMediator_WarnsWhenOverwritten_VerifiesMockObject()
        {
            object oldConfigurator = mapper.ToMediator(typeof(NullMediator));
            mapper.ToMediator(typeof(NullMediator));
            logger.Verify(l => l.LogWarning(It.Is<object>(arg1 => arg1 is string),
                    It.Is<object>(arg2 => arg2 is Type),
                    It.Is<object>(arg3 => arg3 == oldConfigurator)),
                Times.Once);
        }

        [Test]
        public void FromMediator_UnregistersMappingFromHandler_VerifiesMockObject()
        {
            var configurator = mapper.ToMediator<NullMediator>();
            mapper.FromMediator<NullMediator>();
            handler.Verify(h => h.RemoveMapping(It.Is<IMediatorMapping>(arg => arg == configurator)), Times.Once);
        }

        [Test]
        public void FromMediator_RemovesOnlySpecifiedMappingFromHandler_VerifiesMockObject()
        {
            var configurator1 = mapper.ToMediator<NullMediator>();
            var configurator2 = mapper.ToMediator<NullMediator2>();
            mapper.FromMediator<NullMediator>();
            handler.Verify(h => h.RemoveMapping(It.Is<IMediatorMapping>(arg => arg == configurator1)), Times.Once);
            handler.Verify(h => h.RemoveMapping(It.Is<IMediatorMapping>(arg => arg == configurator2)), Times.Never);
        }

        [Test]
        public void FromAll_RemovesAllMappingsFromHandler_VerifiesMockObject()
        {
            var configurator1 = mapper.ToMediator<NullMediator>();
            var configurator2 = mapper.ToMediator<NullMediator2>();
            mapper.FromAll();
            handler.Verify(h => h.RemoveMapping(It.Is<IMediatorMapping>(arg => arg == configurator1)), Times.Once);
            handler.Verify(h => h.RemoveMapping(It.Is<IMediatorMapping>(arg => arg == configurator2)), Times.Once);
        }
    }
}