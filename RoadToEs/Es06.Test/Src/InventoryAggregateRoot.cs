using Es01.Test.Src.Events;
using Es02.Test.Src.Events;
using Es04.Test.Infrastructure;
using System;

namespace Es01.Test.Src
{
    public class InventoryAggregateRoot : AggregateRoot
    {
        
        public InventoryAggregateRoot(Guid id, string name)
        {
            Id = id;
            ApplyChange(new InventoryItemCreated(id, name));
        }

        public InventoryAggregateRoot()
        {

        }

        public void ChangeName(string newName)
        {
            ApplyChange(new ItemNameModified(Id, newName));
        }

        private void Apply(InventoryItemCreated evt)
        {
            Id = evt.Id;
        }
    }
}
