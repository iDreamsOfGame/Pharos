using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Framework;
using Pharos.Framework.Injection;
using PharosEditor.Tests.Supports;

namespace PharosEditor.Tests.Framework
{
    [TestFixture]
    internal class ContextTests
    {
        private IContext context;

        [SetUp]
        public void Setup()
        {
            context = new Context();
        }

        [Test]
        public void CanInstantiate_ReturnIsInstanceOfExpectedInterface()
        {
            Assert.That(context, Is.Not.Null);
            Assert.That(context, Is.InstanceOf<IContext>());
        }

        [Test]
        public void AddExtension_ExtensionAreInstalled_ReturnSameInstance()
        {
            IContext actual = null;
            IExtension extension = new CallbackExtension(delegate(IContext ctx) { actual = ctx; });
            context.AddExtension(extension);
            Assert.That(actual, Is.SameAs(context));
        }

        [Test]
        public void Configure_ConfigAreaConfigured_ReturnIsTrue()
        {
            var installed = false;
            IConfig config = new CallbackConfig(delegate { installed = true; });
            context.Configure(config);
            context.Initialize();
            Assert.That(installed, Is.True);
        }

        [Test]
        public void Injector_InjectorHasMappedIntoItself_ReturnsSameInstance()
        {
            var injector = context.Injector.GetInstance<IInjector>();
            Assert.That(injector, Is.SameAs(context.Injector));
        }

        [Test]
        public void Detain_StoresTheInstance_ReturnExpectedInstance()
        {
            var expected = new object();
            object actual = null;
            context.Detained += delegate(object obj) { actual = obj; };
            context.Detain(expected);
            Assert.That(actual, Is.SameAs(expected));
        }

        [Test]
        public void Release_FreesUpTheInstance_ReturnExpectedInstance()
        {
            var expected = new object();
            object actual = null;
            context.Released += delegate(object obj) { actual = obj; };
            context.Detain(expected);
            context.Release(expected);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void AddChild_SetsChildParentInjector_ReturnsSameInstance()
        {
            var child = new Context();
            context.AddChild(child);
            Assert.That(child.Injector.Parent, Is.SameAs(context.Injector));
        }

        [Test]
        public void AddChild_LogsWarningUnlessChildIsUninitialized_ReturnExpectedWarningMessage()
        {
            LogParams? warning = null;
            context.AddLogHandler(new CallbackLogHandler(
                delegate(LogParams log)
                {
                    if (log.Level == LogLevel.Warning)
                        warning = log;
                }));
            var child = new Context();
            child.Initialize();
            context.AddChild(child);
            Assert.That(warning, Is.Not.Null);
            Assert.That(warning.Value.Message, Does.Contain("must be uninitialized"));
            Assert.That(warning.Value.MessageParameters, Is.EquivalentTo(new object[] { child }));
        }

        [Test]
        public void AddChild_LogsWarningIfChildParentInjectorIsAlreadySet_ReturnExpectedWarningMessage()
        {
            LogParams? warning = null;
            context.AddLogHandler(new CallbackLogHandler(
                delegate(LogParams log)
                {
                    if (log.Level == LogLevel.Warning)
                        warning = log;
                }));
            var child = new Context
            {
                Injector = { Parent = new Injector() }
            };
            context.AddChild(child);

            Assert.That(warning, Is.Not.Null);
            Assert.That(warning.Value.Message, Does.Contain("must not have a parent Injector"));
            Assert.That(warning.Value.MessageParameters, Is.EquivalentTo(new object[] { child }));
        }

        [Test]
        public void RemoveChild_LogsWarningIfChildIsNotAChild_ReturnExpectedWarningMessage()
        {
            LogParams? warning = null;
            context.AddLogHandler(new CallbackLogHandler(
                delegate(LogParams log)
                {
                    if (log.Level == LogLevel.Warning)
                        warning = log;
                }));
            var child = new Context();
            context.RemoveChild(child);

            Assert.That(warning, Is.Not.Null);
            Assert.That(warning.Value.Message, Does.Contain("must be a child"));
            Assert.That(warning.Value.MessageParameters, Is.EquivalentTo(new object[] { child, context }));
        }

        [Test]
        public void RemoveChild_ClearsChildParentInjector_ReturnsNull()
        {
            var child = new Context();
            context.AddChild(child);
            context.RemoveChild(child);
            Assert.That(child.Injector.Parent, Is.Null);
        }

        [Test]
        public void Destroy_ChildIsRemovedWhenChildIsDestroyed_ReturnsNull()
        {
            var child = new Context();
            context.AddChild(child);
            child.Initialize();
            child.Destroy();
            Assert.That(child.Injector.Parent, Is.Null);
        }

        [Test]
        public void Destroy_ChildrenAreRemovedWhenParentIsDestroyed_ReturnsNull()
        {
            var child1 = new Context();
            var child2 = new Context();
            context.AddChild(child1);
            context.AddChild(child2);
            context.Initialize();
            context.Destroy();
            Assert.That(child1.Injector.Parent, Is.Null);
            Assert.That(child2.Injector.Parent, Is.Null);
        }

        [Test]
        public void RemoveChild_ChildIsNotRemovedAgainWhenDestroyed_ReturnsNull()
        {
            LogParams? warning = null;
            context.AddLogHandler(new CallbackLogHandler(
                delegate(LogParams log)
                {
                    if (log.Level == LogLevel.Warning)
                        warning = log;
                }));
            var child = new Context();
            context.AddChild(child);
            child.Initialize();
            context.RemoveChild(child);
            child.Destroy();
            Assert.That(warning, Is.Null);
        }

        [Test]
        public void LifecycleEvents_EventsRaised_ReturnsExpectedCollection()
        {
            var actual = new List<string>();
            var expected = new List<string>
            {
                nameof(context.Initializing),
                nameof(context.Initialized),
                nameof(context.Suspending),
                nameof(context.Suspended),
                nameof(context.Resuming),
                nameof(context.Resumed),
                nameof(context.Destroying),
                nameof(context.Destroyed)
            };
            context.Initializing += OnEventRaised(nameof(context.Initializing));
            context.Initialized += OnEventRaised(nameof(context.Initialized));
            context.Suspending += OnEventRaised(nameof(context.Suspending));
            context.Suspended += OnEventRaised(nameof(context.Suspended));
            context.Resuming += OnEventRaised(nameof(context.Resuming));
            context.Resumed += OnEventRaised(nameof(context.Resumed));
            context.Destroying += OnEventRaised(nameof(context.Destroying));
            context.Destroyed += OnEventRaised(nameof(context.Destroyed));

            context.Initialize();
            context.Suspend();
            context.Resume();
            context.Destroy();
            Assert.That(actual, Is.EqualTo(expected));

            context.Initializing -= OnEventRaised(nameof(context.Initializing));
            context.Initialized -= OnEventRaised(nameof(context.Initialized));
            context.Suspending -= OnEventRaised(nameof(context.Suspending));
            context.Suspended -= OnEventRaised(nameof(context.Suspended));
            context.Resuming -= OnEventRaised(nameof(context.Resuming));
            context.Resumed -= OnEventRaised(nameof(context.Resumed));
            context.Destroying -= OnEventRaised(nameof(context.Destroying));
            context.Destroyed -= OnEventRaised(nameof(context.Destroyed));
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
            context.StateChanged += OnStateChanged;
            context.Initialize();
            Assert.That(hasRaised, Is.True);
            context.StateChanged -= OnStateChanged;
            return;

            void OnStateChanged()
            {
                hasRaised = true;
            }
        }

        [Test]
        public void Initialize_InitializationCallbackIsInvoked_ReturnsPass()
        {
            context.Initialize(delegate { Assert.Pass(); });
            Assert.Fail();
        }

        [Test]
        public void Suspend_SuspensionCallbackIsInvoked_ReturnsPass()
        {
            context.Initialize();
            context.Suspend(delegate { Assert.Pass(); });
            Assert.Fail();
        }

        [Test]
        public void Resume_ResumptionCallbackIsInvoked_ReturnsPass()
        {
            context.Initialize ();
            context.Suspend ();
            context.Resume (delegate { Assert.Pass(); });
            Assert.Fail();
        }

        [Test]
        public void Destroy_DestructionCallbackIsInvoked_ReturnsPass()
        {
            context.Initialize ();
            context.Destroy (delegate { Assert.Pass(); });
            Assert.Fail();
        }
    }
}