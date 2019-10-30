using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Shared
{

    public class RemoveItemsFromInventory
    {
        public Guid InventoryItemId { get; set; }
        public int Count { get; set; }
        public int OriginalVersion { get; set; }

        public RemoveItemsFromInventory(Guid inventoryItemId, int count, int originalVersion)
        {
            InventoryItemId = inventoryItemId;
            Count = count;
            OriginalVersion = originalVersion;
        }
    }
}
