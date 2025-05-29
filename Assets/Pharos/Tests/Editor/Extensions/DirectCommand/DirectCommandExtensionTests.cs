using NUnit.Framework;
using Pharos.Extensions.DirectCommand;
using Pharos.Framework;

namespace PharosEditor.Tests.Extensions.DirectCommand
{
    [TestFixture]
    internal class DirectCommandExtensionTests
    {
        private IContext context;

        [SetUp]
        public void Setup()
        {
            context = new Context();
            context.AddExtension<DirectCommandExtension>();
        }

        [Test]
        public void Enable_DirectCommandMapIsMappedIntoInjector_ReturnsInstanceOfExpectedType()
        {
            object actual = null;
            context.Initializing += OnInitializing;
            context.Initialize();
            Assert.That(actual, Is.InstanceOf<IDirectCommandMap>());
            context.Initializing -= OnInitializing;
            return;

            void OnInitializing(object ctx)
            {
                actual = context.Injector.GetInstance(typeof(IDirectCommandMap));
            }
        }
    }
}