using Es.Lib;
using System;

namespace Inventory.Shared
{
    public class InventoryItemDeactivated: IEvent
    {
        public int Version { get; set; }
        public Guid Id { get; set; }

        public InventoryItemDeactivated(Guid id)
        {
            Id = id;
        }
    }
}
