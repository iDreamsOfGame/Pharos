using NUnit.Framework;
using Pharos.Framework.Injection;
using ReflexPlus.Attributes;

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable MemberHidesStaticFromOuterClass

namespace PharosEditor.Tests.Framework.Injection
{
    [TestFixture]
    internal class InjectorTests
    {
        private interface IFoo
        {
        }

        private interface IFoo2
        {
        }

        private class Foo : IFoo, IFoo2
        {
        }

        private class Foo2 : IFoo
        {
        }

        private class FooContainer
        {
            [Inject(true)]
            internal IFoo Foo { get; private set; }

            [Inject(true, nameof(Foo))]
            internal IFoo FooWithKey { get; private set; }

            [Inject(true)]
            internal IFoo2 Foo2 { get; private set; }
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
        public void HasMapping_BeforeBuilding_ReturnsTrue()
        {
            injector.Map<IFoo>().ToSingleton<Foo>();
            Assert.That(injector.HasMapping<IFoo>(), Is.True);
        }

        [Test]
        public void HasMapping_BeforeBuildingWithKey_ReturnsTrue()
        {
            const string key = nameof(Foo);
            injector.Map<IFoo>(key).ToSingleton<Foo>();
            Assert.That(injector.HasMapping<IFoo>(key), Is.True);
        }

        [Test]
        public void HasMapping_AfterBuilding_ReturnsTrue()
        {
            injector.Map<IFoo>().ToSingleton<Foo>();
            injector.Build();
            Assert.That(injector.HasMapping<IFoo>(), Is.True);
        }

        [Test]
        public void HasMapping_AfterBuildingWithKey_ReturnsTrue()
        {
            const string key = nameof(Foo);
            injector.Map<IFoo>(key).ToSingleton<Foo>();
            injector.Build();
            Assert.That(injector.HasMapping<IFoo>(key), Is.True);
        }

        [Test]
        public void Map_MapInterfaceToValue_ReturnsCorrectInstance()
        {
            var foo = new Foo();
            injector.Map<IFoo>().ToValue(foo);
            injector.Build();
            var instance = injector.GetInstance<IFoo>();
            Assert.That(instance, Is.SameAs(foo));
        }

        [Test]
        public void Map_MapInterfaceToValueWithKey_ReturnsCorrectInstance()
        {
            const string key = nameof(Foo);
            var foo = new Foo();
            injector.Map<IFoo>(key).ToValue(foo);
            injector.Build();
            var instance = injector.GetInstance<IFoo>(key);
            Assert.That(instance, Is.SameAs(foo));
        }

        [Test]
        public void Map_MapClassToValue_ReturnsCorrectInstance()
        {
            var foo = new Foo();
            injector.Map<Foo>().ToValue(foo);
            injector.Build();
            var instance = injector.GetInstance<Foo>();
            Assert.That(instance, Is.SameAs(foo));
        }

        [Test]
        public void Map_MapClassToValueWithKey_ReturnsCorrectInstance()
        {
            const string key = nameof(Foo);
            var foo = new Foo();
            injector.Map<Foo>(key).ToValue(foo);
            injector.Build();
            var instance = injector.GetInstance<Foo>(key);
            Assert.That(instance, Is.SameAs(foo));
        }

        [Test]
        public void Map_MapMultiInterfacesToOneSingletonClass_ReturnsDifferentInstances()
        {
            injector.Map<IFoo>().ToSingleton<Foo>();
            injector.Map<IFoo2>().ToSingleton<Foo>();
            injector.Build();
            var fooContainer = new FooContainer();
            injector.InjectInto(fooContainer);
            Assert.That(fooContainer.Foo, Is.Not.SameAs(fooContainer.Foo2));
        }

        [Test]
        public void Map_MapClassToType_ReturnsDifferentInstances()
        {
            injector.Map<Foo>().ToType<Foo>();
            injector.Build();
            var instance1 = injector.GetInstance<Foo>();
            var instance2 = injector.GetInstance<Foo>();
            Assert.That(instance1, Is.Not.SameAs(instance2));
        }

        [Test]
        public void Map_MapClassToTypeWithKey_ReturnsDifferentInstances()
        {
            const string key = nameof(Foo);
            injector.Map<Foo>(key).ToType<Foo>();
            injector.Build();
            var instance1 = injector.GetInstance<Foo>(key);
            var instance2 = injector.GetInstance<Foo>(key);
            Assert.That(instance1, Is.Not.SameAs(instance2));
        }

        [Test]
        public void Map_MapInterfaceToType_ReturnsNotNullInstance()
        {
            injector.Map<IFoo>().ToType<Foo>();
            injector.Build();
            var fooContainer = new FooContainer();
            injector.InjectInto(fooContainer);
            Assert.That(fooContainer.Foo, Is.Not.Null);
        }

        [Test]
        public void Map_MapInterfaceToTypeWithKey_ReturnsNotNullInstance()
        {
            const string key = nameof(Foo);
            injector.Map<IFoo>(key).ToType<Foo>();
            injector.Build();
            var fooContainer = new FooContainer();
            injector.InjectInto(fooContainer);
            Assert.That(fooContainer.FooWithKey, Is.Not.Null);
        }

        [Test]
        public void Map_MapClassToSingleton_ReturnsCorrectInstance()
        {
            injector.Map<Foo>().ToSingleton<Foo>();
            injector.Build();
            var instance1 = injector.GetInstance<Foo>();
            var instance2 = injector.GetInstance<Foo>();
            Assert.That(instance1, Is.SameAs(instance2));
        }

        [Test]
        public void Map_MapInterfaceToSingleton_ReturnsCorrectInstance()
        {
            injector.Map<IFoo>().ToSingleton<Foo>();
            injector.Build();
            var instance1 = injector.GetInstance<IFoo>();
            var instance2 = injector.GetInstance<IFoo>();
            Assert.That(instance1, Is.SameAs(instance2));
        }

        [Test]
        public void Map_MapSameInterfaceWithDifferentKeysToDifferentSingletonTypes_ReturnsDifferentInstances()
        {
            const string key1 = nameof(Foo);
            const string key2 = nameof(Foo2);
            injector.Map<IFoo>(key1).ToSingleton<Foo>();
            injector.Map<IFoo>(key2).ToSingleton<Foo2>();
            injector.Build();
            var instance1 = injector.GetInstance<IFoo>(key1);
            var instance2 = injector.GetInstance<IFoo>(key2);
            Assert.That(instance1, Is.InstanceOf<Foo>());
            Assert.That(instance2, Is.InstanceOf<Foo2>());
            Assert.That(instance1, Is.Not.SameAs(instance2));
        }

        [Test]
        public void GetOrCreateNewInstance_GetInstanceOnTypeHasMapped_ReturnsCorrectInstance()
        {
            injector.Map<Foo>().ToSingleton<Foo>();
            injector.Build();
            var instance = injector.GetOrCreateNewInstance<Foo>();
            Assert.That(instance, Is.InstanceOf<Foo>());
        }

        [Test]
        public void GetOrCreateNewInstance_GetInstanceOnTypeHasNotMapped_ReturnsNewInstance()
        {
            var instance = injector.GetOrCreateNewInstance<Foo>();
            Assert.That(instance, Is.InstanceOf<Foo>());
        }
    }
}