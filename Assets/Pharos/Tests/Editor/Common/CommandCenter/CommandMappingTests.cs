using System;
using NUnit.Framework;
using Pharos.Common.CommandCenter;
using PharosEditor.Tests.Common.CommandCenter.Supports;

namespace PharosEditor.Tests.Common.CommandCenter
{
    [TestFixture]
    internal class CommandMappingTests
    {
        private CommandMapping mapping;

        private Type commandType;

        [SetUp]
        public void Setup()
        {
            commandType = typeof(NullCommand);
            mapping = new CommandMapping(commandType);
        }

        [Test]
        public void Constructor_MappingStoresCommand_ReturnsExpectedType()
        {
            Assert.That(mapping.CommandType, Is.EqualTo(commandType));
        }

        [Test]
        public void ShouldExecuteOnce_DefaultsToFalse_ReturnsExpectedValue()
        {
            Assert.That(mapping.ShouldExecuteOnce, Is.False);
        }

        [Test]
        public void ShouldExecuteOnce_MappingStoresPropertyValue_ReturnsExpectedValue()
        {
            mapping.ShouldExecuteOnce = true;
            Assert.That(mapping.ShouldExecuteOnce, Is.True);
        }

        [Test]
        public void PayloadInjectionEnabled_DefaultsToTrue_ReturnsExpectedValue()
        {
            Assert.That(mapping.PayloadInjectionEnabled, Is.True);
        }

        [Test]
        public void PayloadInjectionEnabled_MappingStoresPropertyValue_ReturnsExpectedValue()
        {
            mapping.PayloadInjectionEnabled = false;
            Assert.That(mapping.PayloadInjectionEnabled, Is.False);
        }

        [Test]
        public void AddGuards_MappingStoresGuards_ReturnsExpectedGuardsList()
        {
            mapping.AddGuards<GrumpyGuard, HappyGuard>();
            Assert.That(mapping.GuardTypes, Is.EqualTo(new[] { typeof(GrumpyGuard), typeof(HappyGuard) }).AsCollection);
        }

        [Test]
        public void AddGuards_MappingStoresGuardsArray_ReturnsExpectedGuardsList()
        {
            mapping.AddGuards(new[] { typeof(GrumpyGuard), typeof(HappyGuard) });
            Assert.That(mapping.GuardTypes, Is.EqualTo(new[] { typeof(GrumpyGuard), typeof(HappyGuard) }).AsCollection);
        }

        [Test]
        public void AddHooks_MappingStoresHooks_ReturnsExpectedHooksList()
        {
            mapping.AddHooks<NullHook, NullHook2>();
            Assert.That(mapping.HookTypes, Is.EqualTo(new[] { typeof(NullHook), typeof(NullHook2) }).AsCollection);
        }

        [Test]
        public void AddHooks_MappingStoresHooksArray_ReturnsExpectedHooksList()
        {
            mapping.AddHooks(new[] { typeof(NullHook), typeof(NullHook2) });
            Assert.That(mapping.HookTypes, Is.EqualTo(new[] { typeof(NullHook), typeof(NullHook2) }).AsCollection);
        }
    }
}