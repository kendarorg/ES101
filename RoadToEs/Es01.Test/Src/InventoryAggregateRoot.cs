using Es01.Test.Src.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es01.Test.Src
{
    public class InventoryAggregateRoot
    {
        private List<object> _uncommittedChanges = new List<object>();
        

        public InventoryAggregateRoot(Guid id, string name)
        {
            Id = id;
            _uncommittedChanges.Add(new InventoryItemCreated(id, name));
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
    }
}
