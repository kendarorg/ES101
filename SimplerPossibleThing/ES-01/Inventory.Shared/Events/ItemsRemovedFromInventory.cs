using Es.Lib;
using System;

namespace Inventory.Shared
{

    public class ItemsRemovedFromInventory : IEvent
    {
        public int Version { get; set; }
        public Guid Id { get; set; }
        public int Count { get; set; }

        public ItemsRemovedFromInventory(Guid id, int count)
        {
            Id = id;
            Count = count;
        }
    }
}
