using Es.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory.Es
{
    public class TestSnapshot : ISnapshot
    {
        public TestSnapshot()
        {
            ChangedNames = new List<string>();
        }
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public List<string> ChangedNames { get; set; }
    }
}
