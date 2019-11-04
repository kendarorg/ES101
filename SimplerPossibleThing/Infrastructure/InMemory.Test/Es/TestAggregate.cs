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
        public TestAggregate()
        {

        }
        public TestAggregate(Guid id)
        {
            ApplyChange(new TestItemCreated(id));
        }

        public void SetName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName");
            ApplyChange(new TestItemNameAssigned(_id, newName));
        }

        public void Apply(TestItemCreated @event)
        {
            _id = @event.Id;
        }
    }
}
