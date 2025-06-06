using System;
using NUnit.Framework;
using Pharos.Extensions.Mediation;
using Pharos.Framework.Injection;
using PharosEditor.Tests.Extensions.Mediation.Supports;

namespace PharosEditor.Tests.Extensions.Mediation
{
    [TestFixture]
    internal class MediatorViewHandlerTests
    {
        private MediatorViewHandler handler;

        private IInjector injector;
        
        [SetUp]
        public void Setup()
        {
            injector = new Injector();
            handler = new MediatorViewHandler(new MediatorManager(injector));
        }

        [Test]
        public void HandleViewInitialized_ViewIsHandled_ReturnsNotNullInstance()
        {
            IMediator createdMediator = null;
            injector.Map(typeof(Action<object>), "callback").ToValue((Action<object>)Callback);
            var mapping = new MediatorMapping(typeof(SupportView), typeof(CallbackMediator));
            handler.AddMapping(mapping);
            handler.HandleViewInitialized(new SupportView(), typeof(SupportView));
            Assert.That(createdMediator, Is.Not.Null);
            return;

            void Callback(object mediator)
            {
                createdMediator = mediator as IMediator;
            }
        }

        [Test]
        public void HandleViewInitialized_ViewIsNotHandled_ReturnsNull()
        {
            IMediator createdMediator = null;
            injector.Map(typeof(Action<object>), "callback").ToValue((Action<object>)Callback);
            var mapping = new MediatorMapping(typeof(ExtendedSupportView), typeof(CallbackMediator));
            handler.AddMapping(mapping);
            handler.HandleViewInitialized(new SupportView(), typeof(SupportView));
            Assert.That(createdMediator, Is.Null);
            return;

            void Callback(object mediator)
            {
                createdMediator = mediator as IMediator;
            }
        }

        [Test]
        public void HandleViewDestroying_MediatorIsDestroyed_ReturnsNull()
        {
            IMediator createdMediator = null;
            injector.Map(typeof(Action<object>), "callback").ToValue((Action<object>)InitializedCallback);
            injector.Map(typeof(Action<object>), "destroyedCallback").ToValue((Action<object>)DestroyedCallback);
            var mapping = new MediatorMapping(typeof(SupportView), typeof(CallbackMediator));
            handler.AddMapping(mapping);
            var view = new SupportView();
            handler.HandleViewInitialized(view, typeof(SupportView));
            handler.HandleViewDestroying(view);
            Assert.That(createdMediator, Is.Null);
            return;

            void InitializedCallback(object mediator)
            {
                createdMediator = mediator as IMediator;
            }

            void DestroyedCallback(object mediator)
            {
                createdMediator = null;
            }
        }

        [Test]
        public void HandleViewDestroying_MediatorIsNotDestroyed_ReturnsNotNullInstance()
        {
            IMediator createdMediator = null;
            injector.Map(typeof(Action<object>), "callback").ToValue((Action<object>)InitializedCallback);
            injector.Map(typeof(Action<object>), "destroyedCallback").ToValue((Action<object>)DestroyedCallback);
            var mapping = new MediatorMapping(typeof(SupportView), typeof(CallbackMediator));
            handler.AddMapping(mapping);
            handler.HandleViewInitialized(new SupportView(), typeof(SupportView));
            handler.HandleViewDestroying(new ExtendedSupportView());
            Assert.That(createdMediator, Is.Not.Null);
            return;

            void InitializedCallback(object mediator)
            {
                createdMediator = mediator as IMediator;
            }

            void DestroyedCallback(object mediator)
            {
                createdMediator = null;
            }
        }
    }
}