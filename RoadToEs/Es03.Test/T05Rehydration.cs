using System;
using Es01.Test.Src;
using Es01.Test.Src.Commands;
using Es02.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es03.Test
{
    [TestClass]
    public class T05Rehydration
    {
        [TestMethod]
        public void ShouldRehydrate()
        {
            //Given
            Guid id = Guid.NewGuid();
            const string name = "test";
            var target = new EventStore();
            var commandHandler = new InventoryCommandHandler(target);
            commandHandler.Handle(new CreateInventoryItem(id, name));

            //When
            var result = target.GetById<InventoryAggregateRoot>(id);

            //Then
            Assert.AreEqual(id,result.Id);
        }
    }
}
