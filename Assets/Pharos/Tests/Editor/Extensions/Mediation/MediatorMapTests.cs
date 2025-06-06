using NUnit.Framework;
using Pharos.Extensions.Mediation;
using Pharos.Framework;
using PharosEditor.Tests.Extensions.Mediation.Supports;

namespace PharosEditor.Tests.Extensions.Mediation
{
    [TestFixture]
    internal class MediatorMapTests
    {
        private MediatorMap mediatorMap;
        
        [SetUp]
        public void Setup()
        {
            var context = new Context();
            mediatorMap = new MediatorMap(context);
        }

        [Test]
        public void Map_MapViewCreatesMapper_ReturnsInstanceOfExpectedType()
        {
            Assert.That(mediatorMap.Map<SupportView>(), Is.InstanceOf<IMediatorMapper>());
        }

        [Test]
        public void Map_MapSameTypeOfViewReturnsSameMapper_ReturnsObjectEqualToExpected()
        {
            var mapper1 = mediatorMap.Map<SupportView>();
            var mapper2 = mediatorMap.Map<SupportView>();
            Assert.That(mapper1, Is.EqualTo(mapper2));
        }
        
        [Test]
        public void Map_MapDifferentTypeOfViewReturnsDifferentMapper_ReturnsObjectNotEqualToExpected()
        {
            var mapper1 = mediatorMap.Map<SupportView>();
            var mapper2 = mediatorMap.Map<NullView>();
            Assert.That(mapper1, Is.Not.EqualTo(mapper2));
        }

        [Test]
        public void Unmap_UnmapReturnsMapper_ReturnsInstanceOfExpectedType()
        {
            mediatorMap.Map<SupportView>();
            Assert.That(mediatorMap.Unmap<SupportView>(), Is.InstanceOf<IMediatorMapper>());
        }
    }
}