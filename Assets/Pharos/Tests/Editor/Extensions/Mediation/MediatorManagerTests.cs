using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Extensions.Mediation;
using Pharos.Framework.Injection;
using PharosEditor.Tests.Extensions.Mediation.Supports;

namespace PharosEditor.Tests.Extensions.Mediation
{
    [TestFixture]
    internal class MediatorManagerTests
    {
        private IInjector injector;

        private MediatorManager manager;
        
        [SetUp]
        public void Setup()
        {
            injector = new Injector();
            manager = new MediatorManager(injector);
        }

        [Test]
        public void CreateMediator_LifecycleMethodsAreInvoked_ReturnsExpectedCollection()
        {
            var expected = new List<string>
            {
                nameof(LifecycleReportingMediator.PreInitializeCallback),
                nameof(LifecycleReportingMediator.InitializeCallback),
                nameof(LifecycleReportingMediator.PostInitializeCallback),
                nameof(LifecycleReportingMediator.PreDestroyCallback),
                nameof(LifecycleReportingMediator.DestroyCallback),
                nameof(LifecycleReportingMediator.PostDestroyCallback)
            };
            var actual = new List<string>();
            Action<string> callback = delegate(string callbackName) {
                actual.Add(callbackName);
            };
            
            foreach (var callbackName in expected)
            {
                injector.Map<Action<string>>(callbackName).ToValue(callback);
            }
            
            var view = new SupportView();
            var viewType = typeof(SupportView);
            var mapping = new MediatorMapping(viewType, typeof(LifecycleReportingMediator));
            manager.CreateMediator(view, viewType, mapping);
            manager.DestroyMediator(view);
			
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void CreateMediator_ViewIsSet_ReturnsExpectedInstance()
        {
            var expected = new SupportView();
            var viewType = typeof(SupportView);
            var mapping = new MediatorMapping(viewType, typeof(NullMediator));
            var mediator = manager.CreateMediator(expected, viewType, mapping);
            Assert.That(mediator.View, Is.EqualTo(expected));
        }

        [Test]
        public void GetMediator_MediatorIsCreated_ReturnsNotNullInstance()
        {
            var view = new SupportView();
            var viewType = typeof(SupportView);
            var mapping = new MediatorMapping(viewType, typeof(NullMediator));
            manager.CreateMediator(view, viewType, mapping);
            Assert.That(manager.GetMediator(view), Is.Not.Null);
        }

        [Test]
        public void DestroyMediator_MediatorIsDestroyed_ReturnsNull()
        {
            var view = new SupportView();
            var viewType = typeof(SupportView);
            var mapping = new MediatorMapping(viewType, typeof(NullMediator));
            manager.CreateMediator(view, viewType, mapping);
            manager.DestroyMediator(view);
            Assert.That(manager.GetMediator(view), Is.Null);
        }
    }
}