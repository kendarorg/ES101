using System;
using System.Collections.Generic;
using Es01.Test.Src;
using Es01.Test.Src.Commands;
using Es01.Test.Src.Events;
using Es02.Test.Infrastructure;
using Es02.Test.Src.Commands;
using Es02.Test.Src.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es05.Test
{
    [TestClass]
    public class T08TheBus
    {
        [TestMethod]
        public void ShouldSendDataViaBus()
        {
            //Given
            var events = new List<object>();
            Guid id = Guid.NewGuid();
            const string name = "test";
            const string newName = "second";
            var bus = new E05.Test.Infrastructure.Bus();
            bus.AddListener(ob =>
            {
                events.Add(ob);
            });
            var eventStore = new EventStore(bus);
            var target = new InventoryCommandHandler(bus,eventStore);

            //When
            bus.Send(new CreateInventoryItem(id, name));
            bus.Send(new ModifyItemName(id, newName, 0));


            //Then
            Assert.AreEqual(4, events.Count);
            var inventoryItemCreated = events[1] as InventoryItemCreated;

            Assert.AreEqual(id, inventoryItemCreated.Id);
            Assert.AreEqual(name, inventoryItemCreated.Name);

            var itemNameModified = events[3] as ItemNameModified;

            Assert.AreEqual(id, itemNameModified.Id);
            Assert.AreEqual(newName, itemNameModified.NewName);
        }
    }
}
