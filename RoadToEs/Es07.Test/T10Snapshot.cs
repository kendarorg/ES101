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
using Es07.Test.Infrastructure;
using Es07.Test.Src;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Es07.Test
{
#pragma warning disable IDE0059 // Unnecessary assignment of a value
    [TestClass]
    public class T10Snapshot
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
            var snapshotStore = new SnapshotStore();
            var eventStore = new EventStore(bus, snapshotStore);
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
            var snapshotStore = new SnapshotStore();
            var eventStore = new EventStore(bus, snapshotStore);
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



        [TestMethod]
        public void ShouldGenerateSnapshot()
        {
            //Given
            var events = new List<object>();
            Guid id = Guid.NewGuid();
            const string name = "test";
            var bus = new E05.Test.Infrastructure.Bus();
            var snapshotStore = new SnapshotStore();
            var eventStore = new EventStore(bus, snapshotStore);
            var commandHandler = new InventoryCommandHandler(bus, eventStore);
            var projection = new ItemsProjection(bus);
            bus.Send(new CreateInventoryItem(id, name));

            //When
            var i = 0;
            for (; i < 11; i++)
            {
                var lastProjection = projection.GetById(id);
                bus.Send(new ModifyItemName(id, "new"+i, lastProjection.Version));
            }

            //Then
            var items = projection.GetAll().ToList();
            Assert.AreEqual(1, items.Count);
            var kvp = items[0];
            Assert.AreEqual(id, kvp.Id);
            Assert.AreEqual("new10", kvp.Name);
            Assert.AreEqual(11, kvp.Version);

            var snapshotData = snapshotStore.GetSnapshot(id);
            Assert.AreEqual(10, snapshotData.Version);
            var snapshot = JsonConvert.DeserializeObject<InventorySnapshot>(snapshotData.Data);
            Assert.AreEqual("new9", snapshot.Name);

        }
    }
#pragma warning restore IDE0059 // Unnecessary assignment of a value
}
