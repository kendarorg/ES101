using Es.Lib;
using Inventory.Shared.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory.Es
{
    public class SnapshotTestAggregate : AggregateRoot, ISnapshottableAggregate
    {
        private TestSnapshot _memento;
        public override Guid Id { get { return _memento.Id; } }

        public void SetSnasphot(string data)
        {
            _memento = JsonConvert.DeserializeObject<TestSnapshot>(data);
        }

        public string GetSnapshot()
        {
            return JsonConvert.SerializeObject(_memento);
        }
        public void SaveSnapshot()
        {
            ApplyChange(new SnapshotSaved(Id, _memento.Version));
        }

        public SnapshotTestAggregate()
        {

        }
        public SnapshotTestAggregate(Guid id)
        {
            ApplyChange(new TestItemCreated(id));
        }

        public void SetName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName");
            ApplyChange(new TestItemNameAssigned(_memento.Id, newName));
        }

        public void Apply(TestItemCreated @event)
        {
            _memento = new TestSnapshot();
            _memento.Id = @event.Id;
            _memento.Version++;
        }

        public void Apply(TestItemNameAssigned @event)
        {
            _memento.ChangedNames.Add(_memento.Name);
            _memento.Name = @event.Name;
            _memento.Version++;
        }
    }
}
