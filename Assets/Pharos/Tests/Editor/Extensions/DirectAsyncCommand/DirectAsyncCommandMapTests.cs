using System;
using NUnit.Framework;
using Pharos.Extensions.DirectAsyncCommand;
using Pharos.Framework;
using Pharos.Framework.Injection;
using PharosEditor.Tests.Extensions.DirectAsyncCommand.Supports;

namespace PharosEditor.Tests.Extensions.DirectAsyncCommand
{
    [TestFixture]
    internal class DirectAsyncCommandMapTests
    {
        private IContext context;

        private IInjector injector;

        private IDirectAsyncCommandMap subject;

        [SetUp]
        public void Setup()
        {
            context = new Context();
            injector = context.Injector;
            injector.Map<IDirectAsyncCommandMap>().ToType<DirectAsyncCommandMap>();
            subject = injector.GetInstance<IDirectAsyncCommandMap>();
        }

        [Test]
        public void Constructor_SandboxedDirectAsyncCommandMapInstanceDoesNotLeakIntoSystem_ReturnsDifferentInstance()
        {
            var actual = injector.GetInstance<IDirectAsyncCommandMap>();
            Assert.That(actual, Is.Not.EqualTo(subject));
        }

        [Test]
        public void SetCommandsAbortedCallback_EachCommandExecuted_ReturnsExpectedCount()
        {
            var count = 0;
            subject.SetCommandExecutedCallback((_, _, _) => { count++; });
            subject.Map<NullAsyncCommand>().Map<NullAsyncCommand2>().Execute();
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void Map_CreatesInstanceOfIDirectAsyncCommandConfigurator_ReturnsInstanceOfExpectedType()
        {
            Assert.That(subject.Map<NullAsyncCommand>(), Is.InstanceOf<IDirectAsyncCommandConfigurator>());
        }

        [Test]
        public void Map_MappingProcessorIsCalled_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            subject.AddMappingProcessor(delegate { callCount++; });
            subject.Map<NullAsyncCommand>();
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void Execute_AbortsWhenCommandCallsAbortMethod_ReturnsTrue()
        {
            var aborted = false;
            subject.SetCommandsAbortedCallback(() => { aborted = true; });
            subject.Map<NullAsyncCommand>()
                .Map<AbortAsyncCommand>()
                .Map<NullAsyncCommand2>()
                .Execute();
            Assert.That(aborted, Is.True);
        }

        [Test]
        public void Execute_CommandsAllExecuted_ReturnsTrue()
        {
            var commandsExecuted = false;
            subject.SetCommandsExecutedCallback(() => { commandsExecuted = true; });
            subject.Map<NullAsyncCommand>()
                .Map<NullAsyncCommand2>()
                .Execute();
            Assert.That(commandsExecuted, Is.True);
        }

        [Test]
        public void Execute_CommandsGetInjectedWithDirectAsyncCommandMapInstance_ReturnsExpectedInstance()
        {
            IDirectAsyncCommandMap actual = null;
            injector.Map(typeof(Action<IDirectAsyncCommandMap>), "ReportingFunction")
                .ToValue((Action<IDirectAsyncCommandMap>)delegate(IDirectAsyncCommandMap passed) { actual = passed; });
            subject.Map<DirectAsyncCommandMapReportingAsyncCommand>().Execute();
            Assert.That(actual, Is.EqualTo(subject));
        }
    }
}