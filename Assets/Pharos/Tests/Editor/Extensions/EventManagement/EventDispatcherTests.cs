using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Extensions.EventManagement;
using PharosEditor.Tests.Extensions.EventManagement.Supports;

namespace PharosEditor.Tests.Extensions.EventManagement
{
    [TestFixture]
    internal class EventDispatcherTests
    {
        private enum Type
        {
            A,

            B,

            C
        }

        private IEventDispatcher dispatcher;

        private List<object> reported;

        [SetUp]
        public void Setup()
        {
            dispatcher = new EventDispatcher();
            reported = new List<object>();
        }

        [Test]
        public void Dispatch_ListenerGetsCalledAsAction_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            dispatcher.AddEventListener(Type.A, (Action)delegate { callCount++; });
            dispatcher.Dispatch(new BlankEvent(Type.A));
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void Dispatch_ListenerGetsCalledMultipleTimes_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            dispatcher.AddEventListener(Type.A, (Action)delegate { callCount++; });
            dispatcher.Dispatch(new BlankEvent(Type.A));
            dispatcher.Dispatch(new BlankEvent(Type.A));
            dispatcher.Dispatch(new BlankEvent(Type.A));
            Assert.That(callCount, Is.EqualTo(3));
        }

        [Test]
        public void Dispatch_ListenerWontBeCalledIfIncorrectKey_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            dispatcher.AddEventListener(Type.A, (Action)delegate { callCount++; });
            dispatcher.Dispatch(new BlankEvent(Type.B));
            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void Dispatch_ListenerWontBeCalledIfRemoved_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            var action = (Action)delegate { callCount++; };
            dispatcher.AddEventListener(Type.A, action);
            dispatcher.RemoveEventListener(Type.A, action);
            dispatcher.Dispatch(new BlankEvent(Type.A));
            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void Dispatch_AllListenersWontBeCalledIfRemoveAllEventListenersHasCalled_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            var action = (Action)delegate { callCount++; };
            dispatcher.AddEventListener(Type.A, action);
            dispatcher.AddEventListener(Type.B, action);
            dispatcher.AddEventListener(Type.C, action);
            dispatcher.RemoveAllEventListeners();
            dispatcher.Dispatch(new BlankEvent(Type.A));
            dispatcher.Dispatch(new BlankEvent(Type.B));
            dispatcher.Dispatch(new BlankEvent(Type.C));
            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void HasEventListener_AddListenerForHasListener_ReturnsTrue()
        {
            Assert.That(dispatcher.HasEventListener(Type.A), Is.False);
            dispatcher.AddEventListener(Type.A, (Action)delegate { });
            Assert.That(dispatcher.HasEventListener(Type.A), Is.True);
        }

        [Test]
        public void HasEventListener_AddListenerAfterRemovedListener_ReturnsFalse()
        {
            var action = (Action)delegate { };
            dispatcher.AddEventListener(Type.A, action);
            dispatcher.RemoveEventListener(Type.A, action);
            Assert.That(dispatcher.HasEventListener(Type.A), Is.False);
        }

        [Test]
        public void RemoveEventListener_RemovesCorrectTypes_ReturnsExpectedCollection()
        {
            var a = new object();
            var b = new object();
            var c = new object();
            var reportB = Report(b);
            dispatcher.AddEventListener(Type.A, Report(a));
            dispatcher.AddEventListener(Type.B, reportB);
            dispatcher.AddEventListener(Type.C, Report(c));
            dispatcher.RemoveEventListener(Type.B, reportB);
            dispatcher.Dispatch(new BlankEvent(Type.C));
            dispatcher.Dispatch(new BlankEvent(Type.B));
            dispatcher.Dispatch(new BlankEvent(Type.A));
            Assert.That(reported, Is.EqualTo(new List<object> { c, a }).AsCollection);
        }

        [Test]
        public void Dispatch_CheckCustomEventDataGetsPassed_ReturnsExpectedMessage()
        {
            string message = null;
            dispatcher.AddEventListener(CustomEvent.Type.B, delegate(CustomEvent evt) { message = evt.Message; });
            dispatcher.Dispatch(new CustomEvent(CustomEvent.Type.B, "hello world"));
            Assert.That(message, Is.EqualTo("hello world"));
        }

        [Test]
        public void Dispatch_CheckCustomEventCanBeDowncast_ReturnsEventNoNull()
        {
            IEvent actual = null;
            dispatcher.AddEventListener(CustomEvent.Type.B, delegate(IEvent evt) { actual = evt; });
            dispatcher.Dispatch(new CustomEvent(CustomEvent.Type.B, "hello ievent"));
            Assert.That(actual, Is.Not.Null);
        }

        private Action Report(object message) => delegate { reported.Add(message); };
    }
}