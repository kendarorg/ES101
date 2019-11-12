using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es01.Test.Src.Commands
{
    public class CreateInventoryItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public CreateInventoryItem(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
