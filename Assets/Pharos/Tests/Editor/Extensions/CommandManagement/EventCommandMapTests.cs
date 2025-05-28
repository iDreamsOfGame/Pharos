using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Common.CommandCenter;
using Pharos.Extensions.CommandManagement;
using Pharos.Extensions.EventManagement;
using Pharos.Framework;
using Pharos.Framework.Injection;
using PharosEditor.Tests.Common.CommandCenter.Supports;
using PharosEditor.Tests.Extensions.CommandManagement.Supports;

namespace PharosEditor.Tests.Extensions.CommandManagement
{
    [TestFixture]
    internal class EventCommandMapTests
    {
        private enum TestEnum
        {
            Test
        }

        private IEventCommandMap subject;

        private ICommandMapper mapper;

        private List<object> reportedExecutions;

        private IInjector injector;

        private IEventDispatcher dispatcher;

        [SetUp]
        public void Setup()
        {
            reportedExecutions = new List<object>();
            IContext context = new Context();
            injector = context.Injector;
            injector.Map(typeof(Action<object>), "ReportingFunction").ToValue((Action<object>)ReportingFunction);
            dispatcher = new EventDispatcher();
            subject = new EventCommandMap(context, dispatcher);
        }

        [Test]
        public void Map_CreatesMapper_ReturnsInstanceIsExpectedType()
        {
            Assert.That(subject.Map(SupportEvent.Type.Type1, typeof(SupportEvent)), Is.InstanceOf(typeof(ICommandMapper)));
        }

        [Test]
        public void Map_MapToIdenticalTypeButDifferentEvent_ReturnsDifferentMapper()
        {
            mapper = subject.Map(SupportEvent.Type.Type1, typeof(SupportEvent));
            Assert.That(subject.Map(SupportEvent.Type.Type1, typeof(Event)), Is.Not.EqualTo(mapper));
        }

        [Test]
        public void Map_MapToDifferentTypeButIdenticalEvent_ReturnsDifferentMapper()
        {
            mapper = subject.Map(SupportEvent.Type.Type1, typeof(SupportEvent));
            Assert.That(subject.Map(SupportEvent.Type.Type2, typeof(SupportEvent)), Is.Not.EqualTo(mapper));
        }

        [Test]
        public void Unmap_CreateMapper_ReturnsInstanceIsExpectedType()
        {
            mapper = subject.Map(SupportEvent.Type.Type1, typeof(SupportEvent));
            Assert.That(subject.Unmap(SupportEvent.Type.Type1, typeof(SupportEvent)), Is.InstanceOf(typeof(ICommandUnmapper)));
        }

        [Test]
        public void Unmap_RobustToUnmappingNonExistentMappings()
        {
            subject.Unmap(SupportEvent.Type.Type1).FromCommand(typeof(NullCommand));
            // Note: no assertion, just testing for the lack of an NPE.
        }

        [Test]
        public void ToCommand_CommandExecutesSuccessfully_ReturnsExpectedExecutionCount()
        {
            Assert.That(CommandExecutionCount(), Is.EqualTo(1));
        }

        [Test]
        public void ToCommand_CommandExecutesRepeatedly_ReturnsExpectedExecutionCount()
        {
            Assert.That(CommandExecutionCount(5), Is.EqualTo(5));
        }

        [Test]
        public void ExecuteOnce_CommandExecutesOnce_ReturnsExpectedExecutionCount()
        {
            Assert.That(OneshotCommandExecutionCount(5), Is.EqualTo(1));
        }

        [Test]
        public void ToCommand_EventIsInjectedIntoCommand_ReturnsExpectedEventInstance()
        {
            IEvent injectedEvent = null;
            injector.Map(typeof(Action<EventInjectedCallbackCommand>), "ExecuteCallback")
                .ToValue((Action<EventInjectedCallbackCommand>)delegate(EventInjectedCallbackCommand command) { injectedEvent = command.Event; });
            subject.Map(SupportEvent.Type.Type1, typeof(IEvent)).ToCommand<EventInjectedCallbackCommand>();
            var supportEvent = new SupportEvent(SupportEvent.Type.Type1);
            dispatcher.Dispatch(supportEvent);
            Assert.That(injectedEvent, Is.EqualTo(supportEvent));
        }

        [Test]
        public void ToCommand_EventIsPassedToExecuteMethod_ReturnsExpectedEventInstance()
        {
            SupportEvent actualEvent = null;
            injector.Map(typeof(Action<SupportEvent>), "ExecuteCallback")
                .ToValue((Action<SupportEvent>)delegate(SupportEvent supportEvent) { actualEvent = supportEvent; });
            subject.Map(SupportEvent.Type.Type1, typeof(SupportEvent)).ToCommand<EventParametersCommand>();
            var supportEvent = new SupportEvent(SupportEvent.Type.Type1);
            dispatcher.Dispatch(supportEvent);
            Assert.That(actualEvent, Is.EqualTo(supportEvent));
        }

        [Test]
        public void ToCommand_ConcretelySpecifiedTypedEventIsInjectedIntoCommandAsTypedEvent_ReturnsExpectedEventInstance()
        {
            SupportEvent injectedEvent = null;
            injector.Map(typeof(Action<SupportEventTriggeredSelfReportingCallbackCommand>), "ExecuteCallback")
                .ToValue((Action<SupportEventTriggeredSelfReportingCallbackCommand>)delegate(SupportEventTriggeredSelfReportingCallbackCommand command) { injectedEvent = command.TypedEvent; });
            subject.Map(SupportEvent.Type.Type1, typeof(SupportEvent)).ToCommand<SupportEventTriggeredSelfReportingCallbackCommand>();
            var supportEvent = new SupportEvent(SupportEvent.Type.Type1);
            dispatcher.Dispatch(supportEvent);
            Assert.That(injectedEvent, Is.EqualTo(supportEvent));
        }

        [Test]
        public void ToCommand_AbstractlySpecifiedTypedEventIsInjectedIntoCommandAsUntypedEvent_ReturnsExpectedEventInstance()
        {
            IEvent injectedEvent = null;
            injector.Map(typeof(Action<SupportEventTriggeredSelfReportingCallbackCommand>), "ExecuteCallback")
                .ToValue((Action<SupportEventTriggeredSelfReportingCallbackCommand>)delegate(SupportEventTriggeredSelfReportingCallbackCommand command) { injectedEvent = command.UntypedEvent; });
            subject.Map(SupportEvent.Type.Type1, typeof(IEvent)).ToCommand<SupportEventTriggeredSelfReportingCallbackCommand>();
            var supportEvent = new SupportEvent(SupportEvent.Type.Type1);
            dispatcher.Dispatch(supportEvent);
            Assert.That(injectedEvent, Is.EqualTo(supportEvent));
        }

        [Test]
        public void ToCommand_UnspecifiedTypedEventIsInjectedIntoCommandAsTypedEvent_ReturnsExpectedEventInstance()
        {
            SupportEvent injectedEvent = null;
            injector.Map(typeof(Action<SupportEventTriggeredSelfReportingCallbackCommand>), "ExecuteCallback")
                .ToValue((Action<SupportEventTriggeredSelfReportingCallbackCommand>)delegate(SupportEventTriggeredSelfReportingCallbackCommand command) { injectedEvent = command.TypedEvent; });
            subject.Map(SupportEvent.Type.Type1).ToCommand<SupportEventTriggeredSelfReportingCallbackCommand>();
            var supportEvent = new SupportEvent(SupportEvent.Type.Type1);
            dispatcher.Dispatch(supportEvent);
            Assert.That(injectedEvent, Is.EqualTo(supportEvent));
        }

        [Test]
        public void ToCommand_UnspecifiedUntypedEventIsInjectedIntoCommandAsUntypedEvent_ReturnsExpectedEventInstance()
        {
            Enum eventType = TestEnum.Test;
            IEvent injectedEvent = null;
            injector.Map(typeof(Action<SupportEventTriggeredSelfReportingCallbackCommand>), "ExecuteCallback")
                .ToValue((Action<SupportEventTriggeredSelfReportingCallbackCommand>)delegate(SupportEventTriggeredSelfReportingCallbackCommand command) { injectedEvent = command.UntypedEvent; });
            subject.Map(eventType).ToCommand<SupportEventTriggeredSelfReportingCallbackCommand>();
            var e = new Event(eventType);
            dispatcher.Dispatch(e);
            Assert.That(injectedEvent, Is.EqualTo(e));
        }

        [Test]
        public void ToCommand_SpecifiedUntypedEventIsInjectedIntoCommandAsUntypedEvent_ReturnsExpectedEventInstance()
        {
            Enum eventType = TestEnum.Test;
            IEvent injectedEvent = null;
            injector.Map(typeof(Action<SupportEventTriggeredSelfReportingCallbackCommand>), "ExecuteCallback")
                .ToValue((Action<SupportEventTriggeredSelfReportingCallbackCommand>)delegate(SupportEventTriggeredSelfReportingCallbackCommand command) { injectedEvent = command.UntypedEvent; });
            subject.Map(eventType, typeof(IEvent)).ToCommand<SupportEventTriggeredSelfReportingCallbackCommand>();
            var e = new Event(eventType);
            dispatcher.Dispatch(e);
            Assert.That(injectedEvent, Is.EqualTo(e));
        }

        [Test]
        public void ToCommand_CommandDoesNotExecuteWhenIncorrectEventTypeDispatched_ReturnsExpectedExecutionCount()
        {
            uint executionCount = 0;
            injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate { executionCount++; });
            subject.Map(SupportEvent.Type.Type1).ToCommand<CallbackCommand>();
            dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type2));
            Assert.That(executionCount, Is.EqualTo(0));
        }

        [Test]
        public void ToCommand_CommandDoesNotExecuteWhenDifferentEventDispatched_ReturnsExpectedExecutionCount()
        {
            uint executionCount = 0;
            injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate { executionCount++; });
            subject.Map(SupportEvent.Type.Type1, typeof(SupportEvent)).ToCommand<CallbackCommand>();
            dispatcher.Dispatch(new Event(SupportEvent.Type.Type1));
            Assert.That(executionCount, Is.EqualTo(0));
        }

        [Test]
        public void Unmap_CommandDoesNotExecuteAfterEventUnmapped_ReturnsExpectedExecutionCount()
        {
            uint executionCount = 0;
            injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate { executionCount++; });
            subject.Map(SupportEvent.Type.Type1, typeof(SupportEvent)).ToCommand<CallbackCommand>();
            subject.Unmap(SupportEvent.Type.Type1, typeof(SupportEvent)).FromCommand<CallbackCommand>();
            dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            Assert.That(executionCount, Is.EqualTo(0));
        }

        [Test]
        public void ExecuteOnce_OneshotMappingsShouldNotBorkStackedMappings_ReturnsExpectedExecutionCount()
        {
            uint executionCount = 0;
            injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate { executionCount++; });
            subject.Map(SupportEvent.Type.Type1, typeof(SupportEvent)).ToCommand<CallbackCommand>().ExecuteOnce();
            subject.Map(SupportEvent.Type.Type1, typeof(SupportEvent)).ToCommand<CallbackCommand2>().ExecuteOnce();
            dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            Assert.That(executionCount, Is.EqualTo(2));
        }

        [Test]
        public void ExecuteOnce_OneshotMappingsShouldNotCauseInfiniteLoopWhenDispatchingToSelf()
        {
            injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate { dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1)); });
            subject.Map(SupportEvent.Type.Type1).ToCommand<CallbackCommand>().ExecuteOnce();
            dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            // Note: no assertion. we just want to know if an error is thrown.
        }

        [Test]
        public void ExecuteOnce_CommandsShouldNotStompOverEventMappings()
        {
            injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate { dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type2)); });
            subject.Map(SupportEvent.Type.Type1).ToCommand<CallbackCommand>();
            subject.Map(SupportEvent.Type.Type2).ToCommand<CallbackCommand>().ExecuteOnce();
            dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            // Note: no assertion. we just want to know if an error is thrown.
        }

        [Test]
        public void ToCommand_CommandsAreExecutedInOrder_ReturnsExpectedTypeCollection()
        {
            subject.Map(SupportEvent.Type.Type1).ToCommand<ClassReportingCallbackCommand>();
            subject.Map(SupportEvent.Type.Type1).ToCommand<ClassReportingCallbackCommand2>();
            dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            Assert.That(reportedExecutions, Is.EqualTo(new List<object> { typeof(ClassReportingCallbackCommand), typeof(ClassReportingCallbackCommand2) }).AsCollection);
        }

        [Test]
        public void WithHooks_HooksAreCalled_ReturnsExpectedCallCount()
        {
            Assert.That(HookCallCount(typeof(ClassReportingCallbackHook), typeof(ClassReportingCallbackHook)), Is.EqualTo(2));
        }

        [Test]
        public void WithGuards_CommandExecutesWhenTheGuardAllows_ReturnsExpectedExecutionCount()
        {
            Assert.That(CommandExecutionCountWithGuards(typeof(HappyGuard)), Is.EqualTo(1));
        }

        [Test]
        public void WithGuards_CommandExecutesWhenAllGuardsAllow_ReturnsExpectedExecutionCount()
        {
            Assert.That(CommandExecutionCountWithGuards(typeof(HappyGuard), typeof(HappyGuard)), Is.EqualTo(1));
        }

        [Test]
        public void WithGuards_CommandDoesNotExecuteWhenTheGuardDenies_ReturnsExpectedExecutionCount()
        {
            Assert.That(CommandExecutionCountWithGuards(typeof(GrumpyGuard)), Is.EqualTo(0));
        }

        [Test]
        public void WithGuards_CommandDoesNotExecuteWhenAnyGuardDenies_ReturnsExpectedExecutionCount()
        {
            Assert.That(CommandExecutionCountWithGuards(typeof(HappyGuard), typeof(GrumpyGuard)), Is.EqualTo(0));
        }

        [Test]
        public void WithGuards_CommandDoesNotExecuteWhenAllGuardsDeny_ReturnsExpectedExecutionCount()
        {
            Assert.That(CommandExecutionCountWithGuards(typeof(GrumpyGuard), typeof(GrumpyGuard)), Is.EqualTo(0));
        }

        [Test]
        public void WithGuards_EventIsInjectedIntoGuard_ReturnsExpectedEventInstance()
        {
            IEvent injectedEvent = null;
            injector.Map(typeof(Action<EventInjectedCallbackGuard>), "ApproveCallback")
                .ToValue((Action<EventInjectedCallbackGuard>)delegate(EventInjectedCallbackGuard guard) { injectedEvent = guard.Event; });
            subject
                .Map(SupportEvent.Type.Type1, typeof(IEvent))
                .ToCommand<NullCommand>()
                .WithGuards<EventInjectedCallbackGuard>();
            var supportEvent = new SupportEvent(SupportEvent.Type.Type1);
            dispatcher.Dispatch(supportEvent);
            Assert.That(injectedEvent, Is.EqualTo(supportEvent));
        }

        [Test]
        public void ToCommand_CascadingEventsDoNotThrowUnmapErrors()
        {
            injector.Map<IEventDispatcher>().ToValue(dispatcher);
            injector.Map<IEventCommandMap>().ToValue(subject);
            subject.Map(CascadingCommand.EventType.CascadingEvent).ToCommand<CascadingCommand>().ExecuteOnce();
            dispatcher.Dispatch(new Event(CascadingCommand.EventType.CascadingEvent));
        }

        [Test]
        public void WithGuards_ExecutionSequenceIsGuardCommandForMultipleMappingsToSameEvent_ReturnsExpectedOrderCollection()
        {
            subject.Map(SupportEvent.Type.Type1).ToCommand<ClassReportingCallbackCommand>().WithGuards<ClassReportingCallbackGuard>();
            subject.Map(SupportEvent.Type.Type1).ToCommand<ClassReportingCallbackCommand2>().WithGuards<ClassReportingCallbackGuard2>();
            dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            var expectedOrder = new List<object> { typeof(ClassReportingCallbackGuard), typeof(ClassReportingCallbackCommand), typeof(ClassReportingCallbackGuard2), typeof(ClassReportingCallbackCommand2) };
            Assert.That(reportedExecutions, Is.EqualTo(expectedOrder).AsCollection);
        }

        [Test]
        public void WithGuards_PreviouslyConstructedCommandDoesNotSlipThroughTheLoop_ReturnsExpectedOrderCollection()
        {
            subject.Map(SupportEvent.Type.Type1).ToCommand<ClassReportingCallbackCommand>().WithGuards<HappyGuard>();
            subject.Map(SupportEvent.Type.Type1).ToCommand<ClassReportingCallbackCommand2>().WithGuards<GrumpyGuard>();
            dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            var expectedOrder = new List<object> { typeof(ClassReportingCallbackCommand) };
            Assert.That(reportedExecutions, Is.EqualTo(expectedOrder).AsCollection);
        }

        [Test]
        public void Map_CommandsMappedDuringExecutionAreNotExecuted_ReturnsEmptyCollection()
        {
            injector.Map(typeof(IEventCommandMap)).ToValue(subject);
            injector.Map(typeof(Type), "nestedCommand").ToValue(typeof(ClassReportingCallbackCommand));
            subject.Map(SupportEvent.Type.Type1, typeof(IEvent)).ToCommand<CommandMappingCommand>().ExecuteOnce();
            dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            Assert.That(reportedExecutions, Is.Empty);
        }

        [Test]
        public void Unmap_CommandsUnmappedDuringExecutionAreStillExecuted_ReturnsExpectedCollection()
        {
            injector.Map<IEventCommandMap>().ToValue(subject);
            injector.Map(typeof(Type), "nestedCommand").ToValue(typeof(ClassReportingCallbackCommand));
            subject.Map(SupportEvent.Type.Type1, typeof(IEvent)).ToCommand<CommandUnmappingCommand>().ExecuteOnce();
            subject.Map(SupportEvent.Type.Type1, typeof(IEvent)).ToCommand<ClassReportingCallbackCommand>().ExecuteOnce();
            dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            Assert.That(reportedExecutions, Is.EqualTo(new List<object> { typeof(ClassReportingCallbackCommand) }).AsCollection);
        }

        [Test]
        public void AddMappingProcessor_MappingProcessorIsCalled_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            subject.AddMappingProcessor(delegate { callCount++; });
            subject.Map(TestEnum.Test).ToCommand<NullCommand>();
            Assert.That(callCount, Is.EqualTo(1));
        }

        private uint CommandExecutionCount(int totalEvents = 1, bool executeOnce = false)
        {
            uint executeCount = 0;
            injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate { executeCount++; });
            subject.Map(SupportEvent.Type.Type1, typeof(SupportEvent)).ToCommand<CallbackCommand>().ExecuteOnce(executeOnce);
            while (totalEvents-- > 0)
            {
                dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            }

            return executeCount;
        }

        private uint OneshotCommandExecutionCount(int totalEvents = 1)
        {
            return CommandExecutionCount(totalEvents, true);
        }

        private uint HookCallCount(params object[] hooks)
        {
            uint hookCallCount = 0;

            injector.Unmap(typeof(Action<object>), "ReportingFunction");
            injector.Map(typeof(Action<object>), "ReportingFunction").ToValue((Action<object>)delegate { hookCallCount++; });
            subject
                .Map(SupportEvent.Type.Type1)
                .ToCommand<NullCommand>()
                .WithHooks(hooks);
            dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            return hookCallCount;
        }

        private uint CommandExecutionCountWithGuards(params object[] guards)
        {
            uint executionCount = 0;
            injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate { executionCount++; });
            subject
                .Map(SupportEvent.Type.Type1)
                .ToCommand<CallbackCommand>()
                .WithGuards(guards);
            dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.Type1));
            return executionCount;
        }

        private void ReportingFunction(object item)
        {
            reportedExecutions.Add(item);
        }
    }
}