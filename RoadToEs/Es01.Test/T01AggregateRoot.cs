using System;
using Es01.Test.Src;
using Es01.Test.Src.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es01.Test
{
    [TestClass]
    public class T01AggregateRoot
    {
        [TestMethod]
        public void ShouldGenerateEventsOnCreation()
        {
            //Given
            Guid id = Guid.NewGuid();
            const string name = "test";

            //When
            var target = new InventoryAggregateRoot(id, name);

            //Then
            Assert.AreEqual(id, target.Id);
            var changes = target.GetUncommittedChanges();
            Assert.AreEqual(1, changes.Count);
            var inventoryItemCreated = changes[0] as InventoryItemCreated;
            Assert.AreEqual(id, inventoryItemCreated.Id);
            Assert.AreEqual(name, inventoryItemCreated.Name);
        }

        [TestMethod]
        public void ShouldCleanUpEvents()
        {
            //Given
            Guid id = Guid.NewGuid();
            const string name = "test";
            var target = new InventoryAggregateRoot(id, name);

            //When
            target.ClearUncommittedChanges();

            //Then
            var changes = target.GetUncommittedChanges();
            Assert.AreEqual(0, changes.Count);
        }
    }
}
