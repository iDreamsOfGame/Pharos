using NUnit.Framework;
using Pharos.Framework.Helpers;
using Pharos.Framework.Injection;
using ReflexPlus.Attributes;

// ReSharper disable ClassNeverInstantiated.Local

namespace PharosEditor.Tests.Framework.Helpers
{
    [TestFixture]
    internal class ActorsTests
    {
        private class Foo
        {
            [Inject(true)]
            public int InjectedValue { get; private set; }
        }
        
        [Test]
        public void CreateInstance_WithInjectorNotNull_ReturnInstanceNotNull()
        {
            var injector = new Injector();
            injector.Map<int>().ToValue(24);
            var foo = Actors.CreateInstance<Foo>(injector);
            Assert.That(foo, Is.Not.Null);
            Assert.That(foo.InjectedValue, Is.EqualTo(24));
        }

        [Test]
        public void CreateInstance_WithInjectorNull_ReturnInstanceNotNull()
        {
            var foo = Actors.CreateInstance<Foo>();
            Assert.That(foo, Is.Not.Null);
            Assert.That(foo.InjectedValue, Is.EqualTo(0));
        }
    }
}