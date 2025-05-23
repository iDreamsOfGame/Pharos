using NUnit.Framework;
using Pharos.Framework.Injection;

namespace PharosEditor.Tests.Framework.Injection
{
    [TestFixture]
    internal class InjectionMappingTests
    {
        private interface IFoo
        {
        }

        private class Foo : IFoo
        {
        }

        private IInjector injector;

        [SetUp]
        public void Setup()
        {
            injector = new Injector();
        }

        [TearDown]
        public void Cleanup()
        {
            injector.Dispose();
            injector = null;
        }

        [Test]
        public void AsSingleton_MapTypeAsSingletonWithoutKey_ReturnsSameInstance()
        {
            var mapping = new InjectionMapping(injector, typeof(Foo));
            mapping.AsSingleton();
            injector.Build();
            var instance1 = injector.GetInstance<Foo>();
            var instance2 = injector.GetInstance<Foo>();
            Assert.That(instance1, Is.SameAs(instance2));
        }

        [Test]
        public void AsSingleton_MapTypeAsSingletonWithKey_ReturnsSameInstance()
        {
            const string key = nameof(Foo);
            var mapping = new InjectionMapping(injector, typeof(Foo), key);
            mapping.AsSingleton();
            injector.Build();
            var instance1 = injector.GetInstance<Foo>(key);
            var instance2 = injector.GetInstance<Foo>(key);
            Assert.That(instance1, Is.SameAs(instance2));
        }

        [Test]
        public void ToType_MapInterfaceWithoutKey_ReturnsDifferentInstances()
        {
            var mapping = new InjectionMapping(injector, typeof(IFoo));
            mapping.ToType<Foo>();
            injector.Build();
            var instance1 = injector.GetInstance<IFoo>();
            var instance2 = injector.GetInstance<IFoo>();
            Assert.That(instance1, Is.Not.SameAs(instance2));
        }

        [Test]
        public void ToType_MapInterfaceWithKey_ReturnsDifferentInstances()
        {
            const string key = nameof(Foo);
            var mapping = new InjectionMapping(injector, typeof(IFoo), key);
            mapping.ToType<Foo>();
            injector.Build();
            var instance1 = injector.GetInstance<IFoo>(key);
            var instance2 = injector.GetInstance<IFoo>(key);
            Assert.That(instance1, Is.Not.SameAs(instance2));
        }

        [Test]
        public void ToValue_MapInterfaceWithoutKey_ReturnsSameInstance()
        {
            var mapping = new InjectionMapping(injector, typeof(IFoo));
            mapping.ToValue(new Foo());
            injector.Build();
            var instance1 = injector.GetInstance<IFoo>();
            var instance2 = injector.GetInstance<IFoo>();
            Assert.That(instance1, Is.SameAs(instance2));
        }

        [Test]
        public void ToValue_MapInterfaceWithKey_ReturnsSameInstance()
        {
            const string key = nameof(Foo);
            var mapping = new InjectionMapping(injector, typeof(IFoo), key);
            mapping.ToValue(new Foo());
            injector.Build();
            var instance1 = injector.GetInstance<IFoo>(key);
            var instance2 = injector.GetInstance<IFoo>(key);
            Assert.That(instance1, Is.SameAs(instance2));
        }

        [Test]
        public void ToSingleton_MapInterfaceWithoutKey_ReturnsSameInstance()
        {
            var mapping = new InjectionMapping(injector, typeof(IFoo));
            mapping.ToSingleton<Foo>();
            injector.Build();
            var instance1 = injector.GetInstance<IFoo>();
            var instance2 = injector.GetInstance<IFoo>();
            Assert.That(instance1, Is.SameAs(instance2));
        }

        [Test]
        public void ToSingleton_MapInterfaceWithKey_ReturnsSameInstance()
        {
            const string key = nameof(Foo);
            var mapping = new InjectionMapping(injector, typeof(IFoo), key);
            mapping.ToSingleton<Foo>();
            injector.Build();
            var instance1 = injector.GetInstance<IFoo>(key);
            var instance2 = injector.GetInstance<IFoo>(key);
            Assert.That(instance1, Is.SameAs(instance2));
        }
    }
}