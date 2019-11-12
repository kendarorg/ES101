using System;
using System.Collections.Generic;
using System.Linq;
using Es01.Test.Src;
using Es01.Test.Src.Commands;
using Es01.Test.Src.Events;
using Es02.Test.Infrastructure;
using Es02.Test.Src.Commands;
using Es02.Test.Src.Events;
using Es05.Test.Src.Projections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es05.Test
{
    [TestClass]
    public class T09Projections
    {
        [TestMethod]
        public void ShouldPopulateProjections()
        {
            //Given
            var events = new List<object>();
            Guid id = Guid.NewGuid();
            const string name = "test";
            const string newName = "second";
            var bus = new E05.Test.Infrastructure.Bus();
            var eventStore = new EventStore(bus);
            var commandHandler = new InventoryCommandHandler(bus, eventStore);
            var projection = new ItemsProjection(bus);

            //When
            bus.Send(new CreateInventoryItem(id, name));
            bus.Send(new ModifyItemName(id, newName, 0));

            var items = projection.GetAll().ToList();
            Assert.AreEqual(1, items.Count);
            var kvp = items[0];
            Assert.AreEqual(id, kvp.Id);
            Assert.AreEqual(newName, kvp.Name);
            Assert.AreEqual(1, kvp.Version);
        }

        [TestMethod]
        public void ShouldUseTheProjectionToGetTheLatestVersion()
        {
            //Given
            var events = new List<object>();
            Guid id = Guid.NewGuid();
            const string name = "test";
            const string newName = "second";
            var bus = new E05.Test.Infrastructure.Bus();
            var eventStore = new EventStore(bus);
            var commandHandler = new InventoryCommandHandler(bus, eventStore);
            var projection = new ItemsProjection(bus);
            bus.Send(new CreateInventoryItem(id, name));
            var lastProjection = projection.GetById(id);
            //When

            bus.Send(new ModifyItemName(id, newName, lastProjection.Version));

            //Then
            var items = projection.GetAll().ToList();
            Assert.AreEqual(1, items.Count);
            var kvp = items[0];
            Assert.AreEqual(id, kvp.Id);
            Assert.AreEqual(newName, kvp.Name);
            Assert.AreEqual(1, kvp.Version);
        }

    }
}
