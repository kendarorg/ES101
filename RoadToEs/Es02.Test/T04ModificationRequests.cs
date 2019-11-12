using System;
using System.Collections.Generic;
using Es01.Test.Src;
using Es01.Test.Src.Commands;
using Es01.Test.Src.Events;
using Es02.Test.Infrastructure;
using Es02.Test.Src.Commands;
using Es02.Test.Src.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es02.Test
{
    [TestClass]
    public class T04ModificationRequests
    {
        [TestMethod]
        public void ShouldGenerateEventsOnMethodInvocation()
        {
            //Given
            Guid id = Guid.NewGuid();
            const string name = "test";
            const string newName = "second";
            var target = new InventoryAggregateRoot(id,name);

            //When
            target.ChangeName(newName);

            //Then
            var changes = target.GetUncommittedChanges();
            Assert.AreEqual(2, changes.Count);

            var inventoryItemCreated = changes[0] as InventoryItemCreated;
            Assert.AreEqual(id, inventoryItemCreated.Id);
            Assert.AreEqual(name, inventoryItemCreated.Name);

            var itemNameModified = changes[1] as ItemNameModified;
            Assert.AreEqual(id, itemNameModified.Id);
            Assert.AreEqual(newName, itemNameModified.NewName);
        }


        [TestMethod]
        public void ShouldApplyModifications()
        {
            //Given
            Guid id = Guid.NewGuid();
            const string name = "test";
            const string newName = "second";
            var target = new InventoryAggregateRoot();

            var history = new List<object>
            {
                new InventoryItemCreated(id,name),
                new ModifyItemName(id,newName)
            };

            //When
            target.LoadFromHistory(history);

            //Then
            Assert.AreEqual(id, target.Id);
        }


        [TestMethod]
        public void ShouldGenerateNewEventsAfterLoadFromHistory()
        {
            //Given
            Guid id = Guid.NewGuid();
            const string name = "test";
            const string newName = "second";
            const string newNameThree = "third";
            var target = new InventoryAggregateRoot();

            var history = new List<object>
            {
                new InventoryItemCreated(id,name),
                new ModifyItemName(id,newName)
            };
            target.LoadFromHistory(history);

            //When
            target.ChangeName(newNameThree);


            //Then
            var changes = target.GetUncommittedChanges();
            Assert.AreEqual(1, changes.Count);

            var itemNameModified = changes[0] as ItemNameModified;
            Assert.AreEqual(id, itemNameModified.Id);
            Assert.AreEqual(newNameThree, itemNameModified.NewName);
        }
    }
}
