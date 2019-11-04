using System;

namespace Inventory.Shared
{
    public class CheckInItemsToInventory
    {
        public Guid InventoryItemId { get; set; }
        public int Count { get; set; }
        public int OriginalVersion { get; set; }

        public CheckInItemsToInventory(Guid inventoryItemId, int count, int originalVersion)
        {
            InventoryItemId = inventoryItemId;
            Count = count;
            OriginalVersion = originalVersion;
        }
    }
}
