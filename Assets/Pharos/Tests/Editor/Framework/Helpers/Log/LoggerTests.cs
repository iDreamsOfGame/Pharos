using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Framework;
using Pharos.Framework.Helpers;
using PharosEditor.Tests.Framework.Supports;

namespace PharosEditor.Tests.Framework.Helpers
{
    [TestFixture]
    internal class LoggerTests
    {
        private object source;

        private Logger logger;

        [SetUp]
        public void Setup()
        {
            source = new object();
        }

        [Test]
        public void Source_IsValid_ReturnsCorrectInstance()
        {
            var expected = source;
            object actual = null;
            logger = new Logger(source, new CallbackLogHandler(delegate(LogParams result) { actual = result.Source; }));
            logger.LogDebug("hello");
            Assert.That(actual, Is.SameAs(expected));
        }

        [Test]
        public void Level_IsValid_ReturnsCorrectCollection()
        {
            var expected = new[] { LogLevel.FatalError, LogLevel.Error, LogLevel.Warning, LogLevel.Info, LogLevel.Debug };
            var actual = new List<LogLevel>();
            logger = new Logger(source, new CallbackLogHandler(delegate(LogParams result) { actual.Add(result.Level); }));
            logger.LogFatalError("fatal");
            logger.LogError("error");
            logger.LogWarning("warn");
            logger.LogInfo("info");
            logger.LogDebug("debug");
            Assert.That(actual.ToArray(), Is.EqualTo(expected).AsCollection);
        }

        [Test]
        public void Message_IsValid_ReturnsCorrectMessage()
        {
            object expected = "hello";
            object actual = null;
            logger = new Logger(source, new CallbackLogHandler(delegate(LogParams result) { actual = result.Message; }));
            logger.LogDebug(expected);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void MessageParameters_AreValid_ReturnsCorrectMessageParameters()
        {
            var expected = new object[] { 1, 2, 3 };
            object[] actual = null;
            logger = new Logger(source, new CallbackLogHandler(delegate(LogParams result) { actual = result.MessageParameters; }));
            logger.LogDebug("hello", expected);
            Assert.That(actual, Is.EqualTo(expected).AsCollection);
        }
    }
}