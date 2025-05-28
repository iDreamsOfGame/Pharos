using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Pharos.Common.CommandCenter;
using Pharos.Framework;
using PharosEditor.Tests.Common.CommandCenter.Supports;

// ReSharper disable RedundantAssignment
// ReSharper disable NotAccessedVariable
// ReSharper disable UnusedVariable

namespace PharosEditor.Tests.Common.CommandCenter
{
    [TestFixture]
    internal class CommandMappingListTests
    {
        private Mock<ILogger> logger;

        private Mock<ICommandTrigger> trigger;

        private CommandMappingList subject;

        private ICommandMapping mapping1;

        private ICommandMapping mapping2;

        private ICommandMapping mapping3;

        private List<Action<ICommandMapping>> processors;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger>();
            trigger = new Mock<ICommandTrigger>();
            processors = new List<Action<ICommandMapping>>();
            subject = new CommandMappingList(trigger.Object, processors, logger.Object);
            mapping1 = new CommandMapping(typeof(NullCommand));
            mapping2 = new CommandMapping(typeof(NullCommand2));
            mapping3 = new CommandMapping(typeof(NullCommand3));
        }

        [Test]
        public void Constructor_MappingProcessorIsCalled_ReturnsExpectedCallCount()
        {
            var callCount = 0;
            processors.Add(delegate { callCount++; });
            subject.AddMapping(mapping1);
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_MappingProcessorIsGivenMappings_ReturnsExpectedMappingCollection()
        {
            var mappings = new List<ICommandMapping>();
            processors.Add(delegate(ICommandMapping mapping) { mappings.Add(mapping); });
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.AddMapping(mapping3);
            Assert.That(mappings, Is.EqualTo(new List<ICommandMapping> { mapping1, mapping2, mapping3 }).AsCollection);
        }

        [Test]
        public void Mappings_ListIsEmpty_ReturnsZeroListCount()
        {
            Assert.That(subject.Mappings.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Mappings_ListIsNotEmptyAfterMappingAdded_ReturnsExpectedListCount()
        {
            subject.AddMapping(mapping1);
            Assert.That(subject.Mappings.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Mappings_ListHasMapping_ReturnsExpectedIndexOfMappings()
        {
            subject.AddMapping(mapping1);
            Assert.That(Array.IndexOf(subject.Mappings.ToArray(), mapping1), Is.EqualTo(0));
        }

        [Test]
        public void Mappings_ListIsEmptyAfterMappingsAreRemoved_ReturnsZeroListCount()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.RemoveMapping(mapping1);
            subject.RemoveMapping(mapping2);
            Assert.That(subject.Mappings.Count, Is.EqualTo(0));
        }

        [Test]
        public void Mappings_ListIsEmptyAfterRemoveAllMappings_ReturnsZeroListCount()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.AddMapping(mapping3);
            subject.RemoveAllMappings();
            Assert.That(subject.Mappings.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddMapping_TriggerIsActivatedWhenFirstMappingIsAdded_VerifiesMockObject()
        {
            subject.AddMapping(mapping1);
            trigger.Verify(t => t.Activate(), Times.Once);
        }

        [Test]
        public void AddMapping_TriggerIsNotActivatedWhenSecondMappingIsAdded_VerifiesMockObject()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            trigger.Verify(t => t.Activate(), Times.Once);
        }

        [Test]
        public void AddMapping_TriggerIsNotActivatedWhenMappingOverwritten_VerifiesMockObject()
        {
            subject.AddMapping(new CommandMapping(typeof(NullCommand)));
            subject.AddMapping(new CommandMapping(typeof(NullCommand)));
            trigger.Verify(t => t.Activate(), Times.Once);
        }

        [Test]
        public void AddMapping_TriggerIsNotActivatedForSecondIdenticalMapping_VerifiesMockObject()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping1);
            trigger.Verify(t => t.Activate(), Times.Once);
        }

        [Test]
        public void AddMapping_WarningLoggedWhenMappingOverwritten_VerifiesMockObject()
        {
            subject.AddMapping(new CommandMapping(typeof(NullCommand)));
            subject.AddMapping(new CommandMapping(typeof(NullCommand)));
            logger.Verify(r => r.LogWarning(It.IsRegex("already mapped"), It.IsAny<object[]>()), Times.Once);
        }

        [Test]
        public void RemoveMappingFor_TriggerIsDeactivatedWhenLastMappingIsRemoved_VerifiesMockObject()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.RemoveMappingFor(mapping1.CommandType);
            subject.RemoveMappingFor(mapping2.CommandType);
            trigger.Verify(t => t.Deactivate(), Times.Once);
        }

        [Test]
        public void RemoveMappingFor_TriggerIsNotDeactivatedWhenSecondLastMappingIsRemoved_VerifiesMockObject()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.RemoveMappingFor(mapping1.CommandType);
            trigger.Verify(t => t.Deactivate(), Times.Never);
        }

        [Test]
        public void RemoveAllMappings_TriggerIsDeactivatedWhenAllMappingsAreRemoved_VerifiesMockObject()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.AddMapping(mapping3);
            subject.RemoveAllMappings();
            trigger.Verify(t => t.Deactivate(), Times.Once);
        }

        [Test]
        public void RemoveAllMappings_TriggerIsNotDeactivatedWhenListIsAlreadyEmpty_VerifiesMockObject()
        {
            subject.RemoveAllMappings();
            trigger.Verify(t => t.Deactivate(), Times.Never);
        }

        [Test]
        public void WithSortComparer_SortComparerIsUsed_ReturnsExpectedSortedMappings()
        {
            subject.WithSortComparer(new PriorityMappingComparer());
            var priorityMapping1 = new PriorityMapping(typeof(NullCommand), 1);
            var priorityMapping2 = new PriorityMapping(typeof(NullCommand2), 2);
            var priorityMapping3 = new PriorityMapping(typeof(NullCommand3), 3);
            subject.AddMapping(priorityMapping3);
            subject.AddMapping(priorityMapping1);
            subject.AddMapping(priorityMapping2);
            Assert.That(subject.Mappings, Is.EquivalentTo(new List<ICommandMapping> { priorityMapping1, priorityMapping2, priorityMapping3 }));
        }

        [Test]
        public void WithSortComparer_CompareMethodIsCalledAfterMappingsAreAdded_VerifiesMockObject()
        {
            var priorityComparer = new Mock<IComparer<ICommandMapping>>();
            priorityComparer.Setup(c => c.Compare(It.IsAny<ICommandMapping>(), It.IsAny<ICommandMapping>())).Returns(0);
            subject.WithSortComparer(priorityComparer.Object);
            AddPriorityMappings();
            var mappings = subject.Mappings;
            priorityComparer.Verify(c => c.Compare(It.IsAny<ICommandMapping>(), It.IsAny<ICommandMapping>()), Times.AtLeastOnce);
        }

        [Test]
        public void WithSortComparer_CompareMethodIsCalledOnlyOnceAfterMappingsAreAdded_VerifiesMockObject()
        {
            var priorityComparer = new Mock<IComparer<ICommandMapping>>();
            priorityComparer.Setup(c => c.Compare(It.IsAny<ICommandMapping>(), It.IsAny<ICommandMapping>())).Returns(0);
            subject.WithSortComparer(priorityComparer.Object);

            AddPriorityMappings();
            var mappings = subject.Mappings;

            // Reset Times.count to zero
            priorityComparer.Invocations.Clear();

            mappings = subject.Mappings;
            priorityComparer.Verify(c => c.Compare(It.IsAny<ICommandMapping>(), It.IsAny<ICommandMapping>()), Times.Never);
        }

        [Test]
        public void WithSorComparer_CompareMethodIsNotCalledAfterAMappingIsRemoved_VerifiesMockObject()
        {
            var priorityComparer = new Mock<IComparer<ICommandMapping>>();
            priorityComparer.Setup(c => c.Compare(It.IsAny<ICommandMapping>(), It.IsAny<ICommandMapping>())).Returns(0);
            subject.WithSortComparer(priorityComparer.Object);

            AddPriorityMappings();
            var mappings = subject.Mappings;
            priorityComparer.Invocations.Clear();
            subject.RemoveMappingFor(typeof(NullCommand));
            mappings = subject.Mappings;
            priorityComparer.Verify(c => c.Compare(It.IsAny<ICommandMapping>(), It.IsAny<ICommandMapping>()), Times.Never);
        }

        private void AddPriorityMappings()
        {
            subject.AddMapping(new PriorityMapping(typeof(NullCommand), 1));
            subject.AddMapping(new PriorityMapping(typeof(NullCommand2), 2));
            subject.AddMapping(new PriorityMapping(typeof(NullCommand3), 3));
        }
    }
}