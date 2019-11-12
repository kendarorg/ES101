using System;
using Es01.Test.Src;
using Es01.Test.Src.Commands;
using Es01.Test.Src.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es01.Test
{
    [TestClass]
    public class T02CommandHandler
    {
        [TestMethod]
        public void ShouldGenerateEventsOnCreation()
        {
            //Given
            Guid id = Guid.NewGuid();
            const string name = "test";
            var target = new InventoryCommandHandler();

            //When
            target.Handle(new CreateInventoryItem(id,name));

            //Then
            Assert.AreEqual(1, target.Events.Count);
            Assert.AreEqual(id, target.Events[0].Id);
            var inventoryItemCreated = target.Events[0].Data as InventoryItemCreated;
            
            Assert.AreEqual(id, inventoryItemCreated.Id);
            Assert.AreEqual(name, inventoryItemCreated.Name);
        }
    }
}
