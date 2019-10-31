using System;
using System.Collections.Generic;
using System.Linq;
using Bus;
using Es.Lib;
using InMemory.Es;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace InMemory.Es
{
    [TestClass]
    public class AggregateRootTest
    {
        [TestMethod]
        public void ShouldLoadDataFromHistory()
        {
            //Given
            var target = new TestAggregate();
            var id = Guid.NewGuid();
            var history = new List<IEvent>
            {
                new TestItemCreated(id)
            };

            //When
            target.LoadsFromHistory(history);

            //Then
            Assert.AreEqual(id, target.Id);
        }

        [TestMethod]
        public void ShouldGenerateEvents()
        {
            //Given
            var id = Guid.NewGuid();

            //When
            var target = new TestAggregate(id);

            //Then
            var changes = target.GetUncommittedChanges().ToList();
            Assert.AreEqual(1, changes.Count);
            var @event = changes[0] as TestItemCreated;
            Assert.AreEqual(id, @event.Id);
            Assert.AreEqual(0, @event.Version);
        }


        [TestMethod]
        public void ShouldGenerateEventsWithoutApply()
        {
            //Given
            var id = Guid.NewGuid();
            var target = new TestAggregate();
            target.LoadsFromHistory(new List<IEvent>
            {
                new TestItemCreated(id){Version=1}
            });

            //When
            target.SetName("Name");

            //Then
            var changes = target.GetUncommittedChanges().ToList();
            Assert.AreEqual(1, changes.Count);
            var @event = changes[0] as TestItemNameAssigned;
            Assert.AreEqual(id, @event.Id);
            Assert.AreEqual(0, @event.Version);
            Assert.AreEqual("Name", @event.Name);
        }
    }
}