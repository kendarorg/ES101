using Es.Lib;
using System;

namespace Inventory.Shared
{
    public class InventoryItemRenamed : IEvent
    {
        public int Version { get; set; }
        public Guid Id { get; set; }
        public string NewName { get; set; }

        public InventoryItemRenamed(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }
}
