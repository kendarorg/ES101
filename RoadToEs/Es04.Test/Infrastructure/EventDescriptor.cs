using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es02.Test.Infrastructure
{
    public class EventDescriptor
    {
        public Guid Id { get; set; }
        public object Data { get; set; }
        public int Version { get; set; }
    }
}
