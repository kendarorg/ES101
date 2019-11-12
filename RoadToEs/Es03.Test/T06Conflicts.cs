using System;
using Es01.Test.Src;
using Es01.Test.Src.Commands;
using Es02.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es03.Test
{
    [TestClass]
    public class T06Conflicts
    {
        [TestMethod]
        public void UnpredictableResult()
        {
            //Given
            Guid id = Guid.NewGuid();
            const string name = "test";
            var target = new EventStore();
            var commandHandler = new InventoryCommandHandler(target);
            commandHandler.Handle(new CreateInventoryItem(id, name));

            //When
            var user01Result = target.GetById<InventoryAggregateRoot>(id);
            var user02Result = target.GetById<InventoryAggregateRoot>(id);
            user02Result.ChangeName("02");
            user01Result.ChangeName("01");

            //Then
            Assert.Inconclusive("Who will be saved??");
        }
    }
}
