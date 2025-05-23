using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Framework;
using Pharos.Framework.Helpers;

namespace PharosEditor.Tests.Framework.Helpers
{
    [TestFixture]
    internal class LifecycleTransitionTests
    {
        private LifecycleManager lifecycleManager;
        
        private LifecycleTransition transition;
        
        [SetUp]
        public void Setup()
        {
            lifecycleManager = new LifecycleManager(new object());
            transition = new LifecycleTransition(lifecycleManager);
        }

        [TearDown]
        public void Cleanup()
        {
            lifecycleManager = null;
        }

        [Test]
        public void Enter_EnterInvalidTransitionOnInvocationListOfEventIsEmpty_ThrowsException()
        {
            Assert.Throws<Exception>(() => transition.FromStates(LifecycleState.Destroyed).Enter());
        }

        [Test]
        public void Enter_EnterInvalidTransitionOnInvocationListOfEventIsNotEmpty_DoesNotThrowAnyException()
        {
            lifecycleManager.ErrorOccurred += OnErrorOccurred;
            Assert.DoesNotThrow(() => transition.FromStates(LifecycleState.Destroyed).Enter());
            lifecycleManager.ErrorOccurred -= OnErrorOccurred;
            return;

            void OnErrorOccurred(Exception exception)
            {
            }
        }

        [Test]
        public void Enter_OnFinalStateSet_ReturnsExpectedState()
        {
            transition.ToStates(LifecycleState.Initializing, LifecycleState.Activated).Enter();
            Assert.That(lifecycleManager.State, Is.EqualTo(LifecycleState.Activated));
        }

        [Test]
        public void Enter_OnTransitionStateSet_ReturnsExpectedState()
        {
            transition.ToStates(LifecycleState.Initializing, LifecycleState.Activated).Enter();
            transition.ProcessingCallback = () => Assert.That(lifecycleManager.State, Is.EqualTo(LifecycleState.Initializing));
        }

        [Test]
        public void Enter_InstanceCallbacksHasInvoked_ReturnsExpectedCollection()
        {
            var expected = new List<string>
            {
                nameof(LifecycleTransition.PreprocessCallback),
                nameof(LifecycleTransition.ProcessingCallback),
                nameof(LifecycleTransition.PostprocessCallback)
            };
            var actual = new List<string>();
            transition.PreprocessCallback = () => actual.Add(nameof(LifecycleTransition.PreprocessCallback));
            transition.ProcessingCallback = () => actual.Add(nameof(LifecycleTransition.ProcessingCallback));
            transition.PostprocessCallback = () => actual.Add(nameof(LifecycleTransition.PostprocessCallback));
            transition.FromStates(LifecycleState.UnInitialized)
                .ToStates(LifecycleState.Initializing, LifecycleState.Activated)
                .Enter();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Enter_MethodCallbackHasInvoked_ReturnExpectedCallCount()
        {
            var callCount = 0;
            transition.FromStates(LifecycleState.UnInitialized)
                .ToStates(LifecycleState.Initializing, LifecycleState.Activated)
                .Enter(_ => { callCount++; });
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void Enter_MethodCallbackHasInvokedWithExceptionOnInvalidTransition_ReturnsNotNull()
        {
            object actual = null;
            lifecycleManager.ErrorOccurred += OnErrorOccurred;
            transition.FromStates(LifecycleState.Destroying).Enter(exception => actual = exception);
            Assert.That(actual, Is.Not.Null);
            lifecycleManager.ErrorOccurred -= OnErrorOccurred;
            return;
            
            void OnErrorOccurred(Exception exception)
            {
            }
        }

        [Test]
        public void Enter_MethodCallbackHasInvokedOnTransitionAlreadyProcessed_ReturnExpectedCallCount()
        {
            var callCount = 0;
            transition.FromStates(LifecycleState.UnInitialized).ToStates(LifecycleState.Initializing, LifecycleState.Activated);
            transition.Enter();
            transition.Enter(_ => callCount++);
            Assert.That(callCount, Is.EqualTo(1));
        }
    }
}