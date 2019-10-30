using Es.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory.Es
{
    public class TestAggregate : AggregateRoot
    {
        private Guid _id;
        public override Guid Id { get { return _id; } }
    }
}
