using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Framework;
using Pharos.Framework.Helpers;

namespace PharosEditor.Tests.Framework.Helpers
{
    [TestFixture]
    internal class LifecycleManagerTests
    {
        private object target;

        private LifecycleManager lifecycleManager;
        
        [SetUp]
        public void Setup()
        {
            target = new object();
            lifecycleManager = new LifecycleManager(target);
        }

        [Test]
        public void State_Starts_ReturnHasInitializedIsFalse()
        {
            Assert.That(lifecycleManager.State, Is.EqualTo(LifecycleState.UnInitialized));
            Assert.That(lifecycleManager.HasInitialized, Is.False);
        }

        [Test]
        public void Initialize_ChangeStateToActivated_ReturnsHasActivatedIsTrue()
        {
            lifecycleManager.Initialize();
            Assert.That(lifecycleManager.State, Is.EqualTo(LifecycleState.Activated));
            Assert.That(lifecycleManager.HasActivated, Is.True);
        }

        [Test]
        public void Suspend_ChangeStateToSuspended_ReturnsHasSuspendedIsTrue()
        {
            lifecycleManager.Initialize();
            lifecycleManager.Suspend();
            Assert.That(lifecycleManager.State, Is.EqualTo(LifecycleState.Suspended));
            Assert.That(lifecycleManager.HasSuspended, Is.True);
        }
        
        [Test]
        public void Resume_ChangeStateToActivated_ReturnsHasActivatedIsTrue()
        {
            lifecycleManager.Initialize();
            lifecycleManager.Suspend();
            lifecycleManager.Resume();
            Assert.That(lifecycleManager.State, Is.EqualTo(LifecycleState.Activated));
            Assert.That(lifecycleManager.HasActivated, Is.True);
        }

        [Test]
        public void Destroy_ChangeStateToDestroyed_ReturnHasDestroyedIsTrue()
        {
            lifecycleManager.Initialize();
            lifecycleManager.Destroy();
            Assert.That(lifecycleManager.State, Is.EqualTo(LifecycleState.Destroyed));
            Assert.That(lifecycleManager.HasDestroyed, Is.True);
        }

        [Test]
        public void LifecycleEvents_EventsRaised_ReturnsExpectedCollection()
        {
            var actual = new List<string>();
            var expected = new List<string>
            {
                nameof(lifecycleManager.Initializing),
                nameof(lifecycleManager.Initialized),
                nameof(lifecycleManager.Suspending),
                nameof(lifecycleManager.Suspended),
                nameof(lifecycleManager.Resuming),
                nameof(lifecycleManager.Resumed),
                nameof(lifecycleManager.Destroying),
                nameof(lifecycleManager.Destroyed)
            };
            lifecycleManager.Initializing += OnEventRaised(nameof(lifecycleManager.Initializing));
            lifecycleManager.Initialized += OnEventRaised(nameof(lifecycleManager.Initialized));
            lifecycleManager.Suspending += OnEventRaised(nameof(lifecycleManager.Suspending));
            lifecycleManager.Suspended += OnEventRaised(nameof(lifecycleManager.Suspended));
            lifecycleManager.Resuming += OnEventRaised(nameof(lifecycleManager.Resuming));
            lifecycleManager.Resumed += OnEventRaised(nameof(lifecycleManager.Resumed));
            lifecycleManager.Destroying += OnEventRaised(nameof(lifecycleManager.Destroying));
            lifecycleManager.Destroyed += OnEventRaised(nameof(lifecycleManager.Destroyed));
            
            lifecycleManager.Initialize();
            lifecycleManager.Suspend();
            lifecycleManager.Resume();
            lifecycleManager.Destroy();
            Assert.That(actual, Is.EqualTo(expected));
            
            lifecycleManager.Initializing -= OnEventRaised(nameof(lifecycleManager.Initializing));
            lifecycleManager.Initialized -= OnEventRaised(nameof(lifecycleManager.Initialized));
            lifecycleManager.Suspending -= OnEventRaised(nameof(lifecycleManager.Suspending));
            lifecycleManager.Suspended -= OnEventRaised(nameof(lifecycleManager.Suspended));
            lifecycleManager.Resuming -= OnEventRaised(nameof(lifecycleManager.Resuming));
            lifecycleManager.Resumed -= OnEventRaised(nameof(lifecycleManager.Resumed));
            lifecycleManager.Destroying -= OnEventRaised(nameof(lifecycleManager.Destroying));
            lifecycleManager.Destroyed -= OnEventRaised(nameof(lifecycleManager.Destroyed));
            return;

            Action<object> OnEventRaised(string name)
            {
                return delegate { actual.Add(name); };
            }
        }

        [Test]
        public void LifecycleEvents_StateChangedEventRaised_ReturnHasRaisedIsTrue()
        {
            var hasRaised = false;
            lifecycleManager.StateChanged += OnStateChanged;
            lifecycleManager.Initialize();
            Assert.That(hasRaised, Is.True);
            lifecycleManager.StateChanged -= OnStateChanged;
            return;

            void OnStateChanged()
            {
                hasRaised = true;
            }
        }
    }
}