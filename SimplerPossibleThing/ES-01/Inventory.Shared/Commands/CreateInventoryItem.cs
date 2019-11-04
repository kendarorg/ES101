using System;

namespace Inventory.Shared
{
    public class CreateInventoryItem
    {
        public Guid InventoryItemId { get; set; }
        public string Name { get; set; }

        public CreateInventoryItem(Guid inventoryItemId, string name)
        {
            InventoryItemId = inventoryItemId;
            Name = name;
        }
    }
}
