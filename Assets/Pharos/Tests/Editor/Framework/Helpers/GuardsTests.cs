using NUnit.Framework;
using Pharos.Framework;
using Pharos.Framework.Helpers;
using Pharos.Framework.Injection;
using PharosEditor.Tests.Common.CommandCenter.Supports;
using ReflexPlus.Attributes;

// ReSharper disable ClassNeverInstantiated.Local

namespace PharosEditor.Tests.Framework.Helpers
{
    [TestFixture]
    internal class GuardsTests
    {
        private class BossGuard : IGuard
        {
            private readonly bool approve;

            public BossGuard(bool approve)
            {
                this.approve = approve;
            }

            public bool Approve() => approve;
        }

        private class JustTheMiddleManGuard : IGuard
        {
            [Inject]
            public BossGuard BossDecision { get; private set; }

            public bool Approve()
            {
                return BossDecision.Approve();
            }
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
        public void Approve_GrumpyGuardType_ReturnsFalse()
        {
            Assert.That(Guards.Approve(typeof(GrumpyGuard)), Is.False);
        }

        [Test]
        public void Approve_HappyGuardType_ReturnsTrue()
        {
            Assert.That(Guards.Approve(typeof(HappyGuard)), Is.True);
        }

        [Test]
        public void Approve_GuardWithInjectionIfInjectedGuardSaysSo_ReturnsIsFalse()
        {
            injector.Map<BossGuard>().ToValue(new BossGuard(false));
            Assert.That(Guards.Approve(injector, typeof(JustTheMiddleManGuard)), Is.False);
        }

        [Test]
        public void Approve_GuardWithInjectionIfInjectedGuardSaysSo_ReturnsIsTue()
        {
            injector.Map<BossGuard>().ToValue(new BossGuard(true));
            Assert.That(Guards.Approve(injector, typeof(JustTheMiddleManGuard)), Is.True);
        }

        [Test]
        public void Approve_GuardsWithAGrumpyGuardType_ReturnsFalse()
        {
            Assert.That(Guards.Approve(typeof(HappyGuard), typeof(GrumpyGuard)), Is.False);
        }
    }
}