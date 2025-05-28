using NUnit.Framework;
using Pharos.Extensions.CommandManagement;
using Pharos.Extensions.EventManagement;
using Pharos.Framework;

namespace PharosEditor.Tests.Extensions.CommandManagement
{
    [TestFixture]
    internal class CommandManagementExtensionTests
    {
        private Context context;

        [SetUp]
        public void Setup()
        {
            context = new Context();
            context.AddExtension<EventManagementExtension>();
        }

        [Test]
        public void Enable_EventCommandMapIsMappedIntoInjector_ReturnsInstanceOfExpectedType()
        {
            object actual = null;
            context.AddExtension<CommandManagementExtension>();
            context.Initializing += OnInitializing;
            context.Initialize();
            Assert.That(actual, Is.InstanceOf<IEventCommandMap>());
            context.Initializing -= OnInitializing;
            return;

            void OnInitializing(object ctx)
            {
                actual = context.Injector.GetInstance(typeof(IEventCommandMap));
            }
        }
    }
}