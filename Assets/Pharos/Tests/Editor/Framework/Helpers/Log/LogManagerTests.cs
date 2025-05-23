using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Framework;
using Pharos.Framework.Helpers;
using PharosEditor.Tests.Supports;

namespace PharosEditor.Tests.Framework.Helpers
{
    [TestFixture]
    internal class LogManagerTests
    {
        private object source;

        private LogManager logManager;

        [SetUp]
        public void Setup()
        {
            source = new object();
            logManager = new LogManager();
        }

        [Test]
        public void LogLevel_HasSet_ReturnsCorrectLogLevel()
        {
            logManager.LogLevel = LogLevel.Warning;
            Assert.That(logManager.LogLevel, Is.EqualTo(LogLevel.Warning));
        }

        [Test]
        public void Logger_IsDerivedFromILogger_ReturnsCorrectInstanceOfType()
        {
            Assert.That(logManager.GetLogger(source), Is.InstanceOf<ILogger>());
        }

        [Test]
        public void AddLogHandler_AreLoggedTo_ReturnsCorrectCollection()
        {
            var expected = new[] { "target1", "target2", "target3" };
            var actual = new List<string>();
            logManager.AddLogHandler(new CallbackLogHandler(delegate { actual.Add("target1"); }));
            logManager.AddLogHandler(new CallbackLogHandler(delegate { actual.Add("target2"); }));
            logManager.AddLogHandler(new CallbackLogHandler(delegate { actual.Add("target3"); }));
            logManager.GetLogger(source).LogInfo(expected);
            Assert.That(actual, Is.EqualTo(expected).AsCollection);
        }
    }
}