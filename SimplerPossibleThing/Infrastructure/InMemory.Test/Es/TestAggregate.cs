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

        protected override void Initialize()
        {
            RegisterApply<TestItemCreated>(Apply);
            RegisterEvent<TestItemNameAssigned>();
        }

        public TestAggregate() : base()
        {

        }

        public TestAggregate(Guid id) : base()
        {
            ApplyChange(new TestItemCreated(id));
        }

        public void SetName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName");
            ApplyChange(new TestItemNameAssigned(_id, newName));
        }

        private void Apply(TestItemCreated @event)
        {
            _id = @event.Id;
        }
    }
}
