using System;
using System.Linq;
using Inventory.Domain;
using Inventory.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inventory.Test
{
    [TestClass]
    public class InventoryAggregateRootTest
    {
        [TestMethod]
        public void ShouldGenerateTheEventsGivenTheActions()
        {
            //Given
            var id = Guid.NewGuid();
            var name = "name";

            //When
            var target = new InventoryItem(id,name);
            target.CheckIn(10);
            target.Remove(5);
            target.ChangeName("new");
            target.Deactivate();

            //Then
            var allEvents = target.GetUncommittedChanges().ToList();
            var inventoryItemCreated = allEvents[0] as InventoryItemCreated;
            Assert.IsNotNull(inventoryItemCreated);
            var itemsCheckedInToInventory = allEvents[1] as ItemsCheckedInToInventory;
            Assert.IsNotNull(itemsCheckedInToInventory);
            var itemsRemovedFromInventory = allEvents[2] as ItemsRemovedFromInventory;
            Assert.IsNotNull(itemsRemovedFromInventory);
            var inventoryItemRenamed = allEvents[3] as InventoryItemRenamed;
            Assert.IsNotNull(inventoryItemRenamed);
            var inventoryItemDeactivated = allEvents[4] as InventoryItemDeactivated;
            Assert.IsNotNull(inventoryItemDeactivated);
        }
    }
}
