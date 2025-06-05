using NUnit.Framework;
using Pharos.Extensions.Mediation;
using Pharos.Framework;

namespace PharosEditor.Tests.Extensions.Mediation
{
    [TestFixture]
    internal class MediatorMapExtensionTests
    {
        private IContext context;
        
        [SetUp]
        public void Setup()
        {
            context = new Context();
        }

        [Test]
        public void Enable_MediatorMapIsMappedIntoInjector_ReturnsInstanceIsTypeOfExpectedType()
        {
            object actual = null;
            context.AddExtension<MediatorMapExtension>();
            context.Initializing += OnInitializing;
            context.Initialize();
            Assert.That(actual, Is.InstanceOf<IMediatorMap>());
            return;
            
            void OnInitializing(object obj)
            {
                context.Initializing -= OnInitializing;
                actual = context.Injector.GetInstance<IMediatorMap>();
            }
        }

        [Test]
        public void Disable_MediatorMapIsUnmappedFromInjector_ReturnsFalse()
        {
            context.AddExtension<MediatorMapExtension>();
            context.Destroying += OnDestroying;
            context.Initialize();
            context.Destroy();
            return;
            
            void OnDestroying(object obj)
            {
                context.Destroying -= OnDestroying;
                Assert.That(context.Injector.HasMapping<IMediatorMap>(), Is.False);
            }
        }
    }
}