using System;

namespace Inventory.Shared
{
    public class RenameInventoryItem
    {
        public Guid InventoryItemId { get; set; }
        public string NewName { get; set; }
        public int OriginalVersion { get; set; }

        public RenameInventoryItem(Guid inventoryItemId, string newName, int originalVersion)
        {
            InventoryItemId = inventoryItemId;
            NewName = newName;
            OriginalVersion = originalVersion;
        }
    }
}
