
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es01.Test.Src.Events
{
    public class InventoryItemCreated
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public InventoryItemCreated(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
