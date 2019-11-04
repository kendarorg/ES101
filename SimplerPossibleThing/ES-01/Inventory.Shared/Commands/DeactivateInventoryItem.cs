using System;

namespace Inventory.Shared
{
    public class DeactivateInventoryItem
    {
        public Guid InventoryItemId { get; set; }
        public int OriginalVersion { get; set; }

        public DeactivateInventoryItem(Guid inventoryItemId, int originalVersion)
        {
            InventoryItemId = inventoryItemId;
            OriginalVersion = originalVersion;
        }
    }
}
