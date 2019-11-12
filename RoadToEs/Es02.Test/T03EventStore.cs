using System;
using Es01.Test.Src;
using Es01.Test.Src.Commands;
using Es01.Test.Src.Events;
using Es02.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es02.Test
{
    [TestClass]
    public class T03EventStore
    {
        [TestMethod]
        public void ShouldSaveEventsOnCreation()
        {
            //Given
            Guid id = Guid.NewGuid();
            const string name = "test";
            var eventStore = new EventStore();
            var target = new InventoryCommandHandler(eventStore);

            //When
            target.Handle(new CreateInventoryItem(id, name));

            //Then
            Assert.AreEqual(1, eventStore.Events.Count);
            Assert.AreEqual(id, eventStore.Events[0].Id);
            var inventoryItemCreated = eventStore.Events[0].Data as InventoryItemCreated;

            Assert.AreEqual(id, inventoryItemCreated.Id);
            Assert.AreEqual(name, inventoryItemCreated.Name);
        }
    }
}
