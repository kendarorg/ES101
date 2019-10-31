using Es.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory.Es
{
    public class TestItemCreated:IEvent
    {
        public TestItemCreated(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
