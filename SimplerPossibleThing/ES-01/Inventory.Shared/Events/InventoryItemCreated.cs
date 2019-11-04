using Es.Lib;
using System;

namespace Inventory.Shared
{
    public class InventoryItemCreated : IEvent
    {
        public int Version { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public InventoryItemCreated(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
