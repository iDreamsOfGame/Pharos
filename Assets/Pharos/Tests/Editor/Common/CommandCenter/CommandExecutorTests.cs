using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Pharos.Common.CommandCenter;
using Pharos.Framework.Injection;
using PharosEditor.Tests.Common.CommandCenter.Supports;

namespace PharosEditor.Tests.Common.CommandCenter
{
    [TestFixture]
    internal class CommandExecutorTests
    {
        private Mock<UnmapperStub> unmapper;

        private List<ICommandMapping> mappings;

        private CommandExecutor subject;

        private List<object> reported;

        private IInjector injector;

        [SetUp]
        public void Setup()
        {
            unmapper = new Mock<UnmapperStub>();
            reported = new List<object>();
            injector = new Injector();

            injector.Map(typeof(Action<object>), "ReportingFunction").ToValue((Action<object>)ReportingFunction);
            mappings = new List<ICommandMapping>();
            subject = new CommandExecutor(injector);
        }

        [Test]
        public void ExecuteCommands_OneshotMappingIsRemoved_VerifiesMockObject()
        {
            subject = new CommandExecutor(injector, unmapper.Object.Unmap);
            var mapping = AddMapping<TypeReportingCallbackCommand>();
            mapping.ShouldExecuteOnce = true;
            subject.ExecuteCommands(mappings);
            unmapper.Verify(unmapperObject =>
                    unmapperObject.Unmap(It.Is<ICommandMapping>(arg => arg == mapping)),
                Times.Once);
        }

        [Test]
        public void ExecuteCommands_CommandIsExecuted_ReturnsExpectedReportedList()
        {
            AddMapping();
            ExecuteCommands();
            Assert.That(reported, Is.EqualTo(new List<object> { typeof(TypeReportingCallbackCommand) }));
        }

        [Test]
        public void ExecuteCommands_CommandIsExecutedRepeatedly_ReturnsExpectedReportedCount()
        {
            AddMappings(5);
            ExecuteCommands();
            Assert.That(reported.Count, Is.EqualTo(5));
        }

        [Test]
        public void ExecuteCommands_HooksAreCalled_ReturnsExpectedReportedCount()
        {
            AddMapping<NullCommand>()
                .AddHooks(typeof(TypeReportingCallbackHook),
                    typeof(TypeReportingCallbackHook),
                    typeof(TypeReportingCallbackHook));
            ExecuteCommands();
            Assert.That(reported.Count, Is.EqualTo(3));
        }

        [Test]
        public void ExecuteCommands_CommandIsInjectedIntoHook_ReturnsExpectedCommandType()
        {
            SelfReportingCallbackCommand executedCommand = null;
            SelfReportingCallbackCommand injectedCommand = null;
            injector.Map(typeof(Action<SelfReportingCallbackCommand>), "ExecuteCallback")
                .ToValue((Action<SelfReportingCallbackCommand>)delegate(SelfReportingCallbackCommand command) { executedCommand = command; });
            injector.Map(typeof(Action<SelfReportingCallbackHook>), "HookCallback")
                .ToValue((Action<SelfReportingCallbackHook>)delegate(SelfReportingCallbackHook hook) { injectedCommand = hook.Command; });
            AddMapping<SelfReportingCallbackCommand>().AddHooks<SelfReportingCallbackHook>();
            ExecuteCommands();
            Assert.That(injectedCommand, Is.EqualTo(executedCommand));
        }

        [Test]
        public void ExecuteCommands_CommandExecutesWhenTheGuardAllows_ReturnsExpectedReportedList()
        {
            AddMapping().AddGuards<HappyGuard>();
            ExecuteCommands();
            Assert.That(reported, Is.EqualTo(new List<object> { typeof(TypeReportingCallbackCommand) }).AsCollection);
        }

        [Test]
        public void ExecuteCommands_CommandDoesNotExecuteWhenAnyGuardDenies_ReturnsEmptyReportedList()
        {
            AddMapping().AddGuards<HappyGuard, GrumpyGuard>();
            ExecuteCommands();
            Assert.That(reported, Is.Empty);
        }

        [Test]
        public void ExecuteCommands_ExecutionSequenceIsGuardCommandWithMultipleMapping_ReturnsExpectedReportedList()
        {
            AddMapping<TypeReportingCallbackCommand>().AddGuards<TypeReportingCallbackGuard>();
            AddMapping<TypeReportingCallbackCommand2>().AddGuards<TypeReportingCallbackGuard2>();
            ExecuteCommands();
            Assert.That(reported,
                Is.EqualTo(new object[]
                    {
                        typeof(TypeReportingCallbackGuard),
                        typeof(TypeReportingCallbackCommand),
                        typeof(TypeReportingCallbackGuard2),
                        typeof(TypeReportingCallbackCommand2)
                    })
                    .AsCollection);
        }

        [Test]
        public void ExecuteCommands_ExecutionSequenceIsGuardHookCommand_ReturnsExpectedReportedList()
        {
            AddMapping().AddGuards<TypeReportingCallbackGuard>().AddHooks<TypeReportingCallbackHook>();
            ExecuteCommands();
            Assert.That(reported,
                Is.EqualTo(new object[]
                    {
                        typeof(TypeReportingCallbackGuard),
                        typeof(TypeReportingCallbackHook),
                        typeof(TypeReportingCallbackCommand)
                    })
                    .AsCollection);
        }

        [Test]
        public void ExecuteCommands_AllowedCommandsGetExecutedAfterDeniedCommand_ReturnsExpectedReportedList()
        {
            AddMapping<TypeReportingCallbackCommand>().AddGuards<GrumpyGuard>();
            AddMapping<TypeReportingCallbackCommand2>();
            ExecuteCommands();
            Assert.That(reported, Is.EqualTo(new object[] { typeof(TypeReportingCallbackCommand2) }).AsCollection);
        }

        [Test]
        public void ExecuteCommands_ThrowsErrorWhenExecuteMethodIsNotAMethod_ThrowsException()
        {
            Assert.Throws<MissingMethodException>(() =>
            {
                AddMapping<IncorrectExecuteCommand>();
                ExecuteCommands();
            });
        }

        [Test]
        public void ExecuteCommands_PayloadIsInjectedIntoCommand_ReturnsExpectedReportedList()
        {
            AddMapping<PayloadInjectionPointsCommand>();
            var payload = new CommandPayload(new Dictionary<object, Type>
            {
                { "message", typeof(string) },
                { 1, typeof(int) }
            });
            ExecuteCommands(payload);
            Assert.That(reported, Is.EqualTo(payload.ValueTypeMap.Keys).AsCollection);
        }

        [Test]
        public void ExecuteCommands_PayloadIsInjectedIntoHook_ReturnsExpectedReportedList()
        {
            AddMapping<NullCommand>().AddHooks<PayloadInjectionPointsHook>();
            var payload = new CommandPayload(new Dictionary<object, Type>
            {
                { "message", typeof(string) },
                { 1, typeof(int) }
            });
            ExecuteCommands(payload);
            Assert.That(reported, Is.EqualTo(payload.ValueTypeMap.Keys).AsCollection);
        }

        [Test]
        public void ExecuteCommands_PayloadIsInjectedIntoGuard_ReturnsExpectedReportedList()
        {
            AddMapping<NullCommand>().AddGuards<PayloadInjectionPointsGuard>();
            var payload = new CommandPayload(new Dictionary<object, Type>
            {
                { "message", typeof(string) },
                { 1, typeof(int) }
            });
            ExecuteCommands(payload);
            Assert.That(reported, Is.EqualTo(payload.ValueTypeMap.Keys).AsCollection);
        }

        [Test]
        public void ExecuteCommands_PayloadIsPassedToExecuteMethod_ReturnsExpectedReportedList()
        {
            AddMapping<MethodParametersCommand>();
            var payload = new CommandPayload(new Dictionary<object, Type>
            {
                { "message", typeof(string) },
                { 1, typeof(int) }
            });
            ExecuteCommands(payload);
            Assert.That(reported, Is.EqualTo(payload.ValueTypeMap.Keys).AsCollection);
        }

        [Test]
        public void ExecuteCommands_PayloadInjectionIsDisabled_ReturnsExpectedReportedList()
        {
            AddMapping<OptionalInjectionPointsCommand>().PayloadInjectionEnabled = false;
            var payload = new CommandPayload(new Dictionary<object, Type>
            {
                { "message", typeof(string) },
                { 1, typeof(int) }
            });
            ExecuteCommands(payload);
            Assert.That(reported, Is.EqualTo(new object[] { null, 0 }).AsCollection);
        }

        [Test]
        public void ExecuteCommands_PayloadDoesNotLeakIntoTypeInstantiatedByCommand_ReturnsExpectedReportedList()
        {
            injector.Map<IInjector>().ToValue(injector);
            AddMapping<OptionalInjectionPointsCommandInstantiatingCommand>();
            var payload = new CommandPayload(new Dictionary<object, Type>
            {
                { "message", typeof(string) },
                { 1, typeof(int) }
            });
            ExecuteCommands(payload);
            Assert.That(reported, Is.EqualTo(new object[] { null, 0 }).AsCollection);
        }

        [Test]
        public void ExecuteCommands_ResultIsHandled_ReturnsExpectedObjectsAndValues()
        {
            var mapping = new CommandMapping(typeof(MessageReturningCommand));
            subject = new CommandExecutor(injector, null, null, ResultReporter);
            injector.Map<string>().ToValue("message");
            subject.ExecuteCommand(mapping);
            Assert.That(reported.Count, Is.EqualTo(1));
            Assert.That(reported[0], Is.InstanceOf<Dictionary<string, object>>());
            if (reported[0] is Dictionary<string, object> reportedDict)
            {
                Assert.That(reportedDict["result"], Is.EqualTo("message"));
                Assert.That(reportedDict["command"], Is.InstanceOf<MessageReturningCommand>());
                Assert.That(reportedDict["mapping"], Is.EqualTo(mapping));
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void ExecuteCommands_UsesInjectorMappedCommandInstance_ReturnsExpectedReportedList()
        {
            injector.Map(typeof(Action<SelfReportingCallbackCommand>), "ExecuteCallback").ToValue((Action<SelfReportingCallbackCommand>)ReportingFunction);
            injector.Map<SelfReportingCallbackCommand>().AsSingleton();
            var expected = injector.GetInstance<SelfReportingCallbackCommand>();
            var mapping = AddMapping(typeof(SelfReportingCallbackCommand));
            subject.ExecuteCommand(mapping);
            Assert.That(reported, Is.EqualTo(new object[] { expected }).AsCollection);
        }

        [Test]
        public void ExecuteCommands_CommandMappedToInterfaceIsExecuted_ReturnsExpectedReportedList()
        {
            injector.Map<ICommand> ().ToType (typeof(AbstractInterfaceImplementingCommand));
            subject.ExecuteCommand(AddMapping<ICommand>());
            Assert.That(reported, Is.EqualTo(new object[]{typeof(AbstractInterfaceImplementingCommand)}).AsCollection);
        }

        private ICommandMapping AddMapping<T>()
        {
            return AddMapping(typeof(T));
        }

        private ICommandMapping AddMapping(Type commandType = null)
        {
            commandType ??= typeof(TypeReportingCallbackCommand);
            var mapping = new CommandMapping(commandType);
            mappings.Add(mapping);
            return mapping;
        }

        private void AddMappings(uint totalEvents = 1, Type commandType = null)
        {
            while (totalEvents-- > 0)
            {
                AddMapping(commandType);
            }
        }

        private void ExecuteCommands(CommandPayload payload = default)
        {
            subject.ExecuteCommands(mappings, payload);
        }

        private void ReportingFunction(object item)
        {
            reported.Add(item);
        }

        private void ResultReporter(object result, object command, ICommandMapping mapping)
        {
            var map = new Dictionary<string, object>
            {
                { "result", result },
                { "command", command },
                { "mapping", mapping }
            };
            reported.Add(map);
        }
    }
}