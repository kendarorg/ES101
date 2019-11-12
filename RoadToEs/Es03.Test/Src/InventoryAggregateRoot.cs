using Es01.Test.Src.Events;
using Es02.Test.Infrastructure;
using Es02.Test.Src.Events;
using Es03.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es01.Test.Src
{
    public class InventoryAggregateRoot : IAggregateRoot
    {
        private List<object> _uncommittedChanges = new List<object>();


        public InventoryAggregateRoot(Guid id, string name)
        {
            Id = id;
            _uncommittedChanges.Add(new InventoryItemCreated(id, name));
        }

        public InventoryAggregateRoot()
        {
        }

        public void ChangeName(string newName)
        {
            _uncommittedChanges.Add(new ItemNameModified(Id, newName));
        }

        public Guid Id { get; private set; }

        public List<object> GetUncommittedChanges()
        {
            return _uncommittedChanges;
        }

        public void ClearUncommittedChanges()
        {
            _uncommittedChanges.Clear();
        }

        public void LoadFromHistory(IEnumerable<object> events)
        {
            foreach (var evt in events)
            {
                if (evt is InventoryItemCreated)
                {
                    Apply((InventoryItemCreated)evt);
                }
            }
        }

        private void Apply(InventoryItemCreated evt)
        {
            Id = evt.Id;
        }
    }
}
