using Es07.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es07.Test.Src
{
    public class InventorySnapshot : ISnapshot
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
    }
}
