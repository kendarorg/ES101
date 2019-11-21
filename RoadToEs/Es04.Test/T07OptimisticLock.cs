using System;
using System.Collections.Generic;
using Es01.Test.Src;
using Es01.Test.Src.Commands;
using Es01.Test.Src.Events;
using Es02.Test.Infrastructure;
using Es02.Test.Src.Commands;
using Es02.Test.Src.Events;
using Es04.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es04.Test
{
    [TestClass]
    public class T07OptimisticLock
    {

        [TestMethod]
        public void ShouldSaveEventsOnMOdificationWithVersion()
        {
            //Given
            Guid id = Guid.NewGuid();
            const string name = "test";
            const string newName = "second";
            var eventStore = new EventStore();
            var target = new InventoryCommandHandler(eventStore);

            //When
            target.Handle(new CreateInventoryItem(id, name));
            target.Handle(new ModifyItemName(id, newName, 0));


            //Then
            Assert.AreEqual(2, eventStore.Events.Count);
            Assert.AreEqual(id, eventStore.Events[0].Id);
            Assert.AreEqual(0, eventStore.Events[0].Version);
            var inventoryItemCreated = eventStore.Events[0].Data as InventoryItemCreated;

            Assert.AreEqual(id, inventoryItemCreated.Id);
            Assert.AreEqual(name, inventoryItemCreated.Name);

            Assert.AreEqual(id, eventStore.Events[1].Id);
            Assert.AreEqual(1, eventStore.Events[1].Version);
            var itemNameModified = eventStore.Events[1].Data as ItemNameModified;

            Assert.AreEqual(id, itemNameModified.Id);
            Assert.AreEqual(newName, itemNameModified.NewName);

        }


        [TestMethod]
        public void ShouldThrowExceptionWithWrongVersion()
        {
            //Given
            Guid id = Guid.NewGuid();
            const string name = "test";
            const string newName = "second";
            const int wrongExpectedVersion = 1;
            var eventStore = new EventStore();
            var target = new InventoryCommandHandler(eventStore);

            //When
            target.Handle(new CreateInventoryItem(id, name));
            Assert.ThrowsException<ConcurrencyException>(()=>
                target.Handle(new ModifyItemName(id, newName, wrongExpectedVersion)));
        }
    }
}
