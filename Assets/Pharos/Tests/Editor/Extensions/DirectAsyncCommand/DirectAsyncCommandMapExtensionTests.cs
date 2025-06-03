using NUnit.Framework;
using Pharos.Extensions.DirectAsyncCommand;
using Pharos.Framework;

namespace PharosEditor.Tests.Extensions.DirectAsyncCommand
{
    [TestFixture]
    internal class DirectAsyncCommandMapExtensionTests
    {
        private IContext context;
        
        [SetUp]
        public void Setup()
        {
            context = new Context();
            context.AddExtension<DirectAsyncCommandMapExtension>();
        }

        [TearDown]
        public void Cleanup()
        {
            context.Destroy();
        }

        [Test]
        public void Enable_DirectAsyncCommandMapIsMappedIntoInjector_ReturnsInstanceOfExpectedType()
        {
            object actual = null;
            context.Initializing += OnInitializing;
            context.Initialize();
            Assert.That(actual, Is.InstanceOf<IDirectAsyncCommandMap>());
            context.Initializing -= OnInitializing;
            return;

            void OnInitializing(object ctx)
            {
                actual = context.Injector.GetInstance(typeof(IDirectAsyncCommandMap));
            }
        }
    }
}