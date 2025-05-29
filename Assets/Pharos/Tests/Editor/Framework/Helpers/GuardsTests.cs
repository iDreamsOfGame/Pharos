using System;
using System.Collections.Generic;
using NUnit.Framework;
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
        private class BossGuard
        {
            private readonly bool approve;

            public BossGuard(bool approve)
            {
                this.approve = approve;
            }

            public bool Approve() => approve;
        }

        private class JustTheMiddleManGuard
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
        public void Approve_GrumpyFunction_ReturnsFalse()
        {
            Assert.That(Guards.Approve(new object[] { (Func<bool>)GrumpyFunction }), Is.False);
        }

        [Test]
        public void Approve_HappyFunction_ReturnsTrue()
        {
            Assert.That(Guards.Approve(HappyFunction), Is.True);
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
        public void Approve_GrumpyGuardInstance_ReturnsFalse()
        {
            Assert.That(Guards.Approve(new GrumpyGuard()), Is.False);
        }

        [Test]
        public void Approve_HappyGuardInstance_ReturnsTrue()
        {
            Assert.That(Guards.Approve(new HappyGuard()), Is.True);
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

        [Test]
        public void Approve_GuardsWithAGrumpyGuardInstance_ReturnsFalse()
        {
            Assert.That(Guards.Approve(new HappyGuard(), new GrumpyGuard()), Is.False);
        }

        [Test]
        public void Approve_GuardsWithAGrumpyFunction_ReturnsFalse()
        {
            Assert.That(Guards.Approve(HappyFunction, GrumpyFunction), Is.False);
        }

        [Test]
        public void Approve_FalseFunction_ReturnsFalse()
        {
            Assert.That(Guards.Approve(FalseGuard), Is.False);
            return;
            bool FalseGuard() => false;
        }

        [Test]
        public void Approve_TrueFunction_ReturnsTrue()
        {
            Assert.That(Guards.Approve(TrueGuard), Is.True);
            return;
            bool TrueGuard() => true;
        }

        [Test]
        public void Approve_FunctionList_ReturnsTrue()
        {
            Assert.That(Guards.Approve(new List<Func<bool>> { TrueGuard, HappyFunction }), Is.True);
            return;
            bool TrueGuard() => true;
        }

        [Test]
        public void Approve_GuardInstanceWithoutApprove_ThrowsException()
        {
            Assert.Throws<MissingMethodException>(() =>
            {
                var invalidGuard = new object();
                Guards.Approve(invalidGuard);
            });
        }

        private static bool HappyFunction() => true;

        private static bool GrumpyFunction() => false;
    }
}