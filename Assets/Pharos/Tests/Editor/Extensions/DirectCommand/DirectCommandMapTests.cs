using System;
using NUnit.Framework;
using Pharos.Extensions.DirectCommand;
using Pharos.Framework;
using Pharos.Framework.Injection;
using PharosEditor.Tests.Common.CommandCenter.Supports;
using ReflexPlus.Attributes;

// ReSharper disable ClassNeverInstantiated.Local

namespace PharosEditor.Tests.Extensions.DirectCommand
{
    [TestFixture]
    internal class DirectCommandMapTests
    {
        private class DirectCommandMapReportingCommand
        {
            [Inject]
            public IDirectCommandMap DirectCommandMap { get; private set; }

            [Inject("ReportingFunction")]
            public Action<IDirectCommandMap> ReportingFunc { get; private set; }

            public void Execute()
            {
                ReportingFunc?.Invoke(DirectCommandMap);
            }
        }

        private IContext context;

        private IDirectCommandMap subject;

        private IInjector injector;

        [SetUp]
        public void Setup()
        {
            context = new Context();
            injector = context.Injector;
            injector.Map<IDirectCommandMap>().ToType<DirectCommandMap>();
            subject = injector.GetInstance<IDirectCommandMap>();
        }

        [Test]
        public void Map_CreatesIDirectCommandConfigurator_ReturnsInstanceIsExpectedType()
        {
            Assert.That(subject.Map<NullCommand>(), Is.InstanceOf<IDirectCommandConfigurator>());
        }

        [Test]
        public void Execute_SuccessfullyExecutesCommands_ReturnsExpectedExecutionCount()
        {
            var executionCount = 0;
            injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate { executionCount++; });
            subject.Map<CallbackCommand>()
                .Map<CallbackCommand2>()
                .Execute();
            Assert.That(executionCount, Is.EqualTo(2));
        }

        [Test]
        public void Execute_CommandsGetInjectedWithDirectCommandMapInstance_ReturnsExpectedInstance()
        {
            IDirectCommandMap actual = null;
            injector.Map(typeof(Action<IDirectCommandMap>), "ReportingFunction").ToValue((Action<IDirectCommandMap>)delegate(IDirectCommandMap passed) { actual = passed; });
            subject.Map<DirectCommandMapReportingCommand>().Execute();
            Assert.That(actual, Is.EqualTo(subject));
        }

        [Test]
        public void Execute_CommandsAreDisposedAfterExecution_ReturnsExpectedExecutionCount()
        {
            var executionCount = 0;
            injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate { executionCount++; });
            subject.Map<CallbackCommand>().Execute();
            subject.Map<CallbackCommand>().Execute();
            Assert.That(executionCount, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_SandboxedDirectCommandMapInstanceDoesNotLeakIntoSystem_ReturnsDifferentInstance()
        {
            var actual = injector.GetInstance<IDirectCommandMap>();
            Assert.That(actual, Is.Not.EqualTo(subject));
        }

        [Test]
        public void Detain_DetainsCommand_ReturnsFlagHasDetainedIsTrue()
        {
            var command = new object();
            var hasDetained = false;
            context.Detained += OnDetained;
            subject.Detain(command);
            Assert.That(hasDetained, Is.True);
            context.Detained -= OnDetained;
            return;
            
            void OnDetained(object obj)
            {
                hasDetained = true; 
            }
        }
        
        [Test]
        public void Release_ReleasesCommand_ReturnsFlagHasReleasedIsTrue()
        {
            var command = new object();
            var hasReleased = false;
            context.Released += OnReleased;
            subject.Detain(command);
            subject.Release(command);
            Assert.That(hasReleased, Is.True);
            context.Released -= OnReleased;
            return;
            
            void OnReleased(object obj)
            {
                hasReleased = true; 
            }
        }

        [Test]
        public void Execute_ExecutesCommands_ReturnsExpectedExecutionCount()
        {
            var executionCount = 0;
            injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate { executionCount++; });
            subject.Map<CallbackCommand>();
            subject.Execute();
            Assert.That(executionCount, Is.EqualTo(1));
        }

        [Test]
        public void AddMappingProcessor_MappingProcessorIsCalled_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            subject.AddMappingProcessor(delegate { callCount++; });
            subject.Map<NullCommand>();
            Assert.That(callCount, Is.EqualTo(1));
        }
    }
}