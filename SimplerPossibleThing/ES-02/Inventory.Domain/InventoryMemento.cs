using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain
{
    public class InventoryMemento
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Activated { get; set; }
        public int Count { get; set; }
    }
}
