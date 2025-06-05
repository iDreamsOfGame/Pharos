using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pharos.Common.CommandCenter;

namespace PharosEditor.Tests.Common.CommandCenter
{
    [TestFixture]
    internal class CommandPayloadTests
    {
        private CommandPayload subject;

        [Test]
        public void Constructor_ValuesDefaultIsNull_ReturnsNull()
        {
            CreateConfig();
            Assert.That(subject.Values, Is.Null);
        }

        [Test]
        public void Constructor_TypesDefaultIsNull_ReturnsNull()
        {
            CreateConfig();
            Assert.That(subject.Types, Is.Null);
        }

        [Test]
        public void Constructor_ValueTypeMapIsStored_ReturnsExpectedCollections()
        {
            var expected = new Dictionary<object, Type>
            {
                { "string", typeof(string) },
                { 1, typeof(int) }
            };
            CreateConfig(expected);
            Assert.That(subject.ValueToType, Is.EqualTo(expected));
        }

        [Test]
        public void AddPayload_StoresValueTypePair_ReturnsFlagHasPayloadTrue()
        {
            CreateConfig();
            subject.AddPayload("string", typeof(string));
            var hasValue = subject.Values.Contains("string");
            var hasType = subject.Types.Contains(typeof(string));
            Assert.That(hasValue, Is.True);
            Assert.That(hasType, Is.True);
        }

        [Test]
        public void AddPayload_AddingStoresInLockstep_ReturnsSameIndexValues()
        {
            var valueToType = new Dictionary<object, Type>
            {
                { "string", typeof(string) },
                { 1, typeof(int) }
            };
            CreateConfig(valueToType);
            const float value = 5f;

            subject.AddPayload(value, typeof(float));

            var valueIndex = subject.Values.IndexOf(value);
            var classIndex = subject.Types.IndexOf(typeof(float));
            Assert.That(valueIndex, Is.EqualTo(classIndex));
        }

        private void CreateConfig(Dictionary<object, Type> valueToType = null)
        {
            subject = new CommandPayload(valueToType);
        }
    }
}