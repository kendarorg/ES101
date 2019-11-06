using Es.Lib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Shared.Commands
{
    public class CreateInventorySnapshot : ICommand
    {
        public Guid InventoryItemId { get; set; }
        public int OriginalVersion { get; set; }
    }
}
