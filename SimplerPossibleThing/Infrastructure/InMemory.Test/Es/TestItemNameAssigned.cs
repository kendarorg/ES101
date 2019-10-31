using Es.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory.Es
{
    public class TestItemNameAssigned : IEvent
    {
        public TestItemNameAssigned(Guid id,string name)
        {
            Id = id;
            Name = name;
        }
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
    }
}
