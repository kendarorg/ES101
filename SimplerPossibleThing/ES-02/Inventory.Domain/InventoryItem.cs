using Es.Lib;
using Inventory.Shared;
using Inventory.Shared.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain
{
    public class InventoryItem : AggregateRoot, ISnapshottableAggregate
    {
        private InventorySnapshot _memento;

        public override Guid Id
        {
            get { return _memento.Id; }
        }

        public void SetSnasphot(string data)
        {
            _memento = JsonConvert.DeserializeObject<InventorySnapshot>(data);
        }

        public string GetSnapshot()
        {
            return JsonConvert.SerializeObject(_memento);
        }

        
        public InventoryItem()
        {

        }

        public InventoryItem(Guid id, string name)
        {
            ApplyChange(new InventoryItemCreated(id, name));
        }

        public void SaveSnapshot()
        {
            ApplyChange(new SnapshotSaved(Id,_memento.Version));
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName");
            ApplyChange(new InventoryItemRenamed(Id, newName));
        }
        
        public void CheckIn(int count)
        {
            if (count <= 0) throw new InvalidOperationException("must have a count greater than 0 to add to inventory");
            ApplyChange(new ItemsCheckedInToInventory(Id, count));
        }

        public void Remove(int count)
        {
            if (count <= 0) throw new InvalidOperationException("cant remove negative count from inventory");
            ApplyChange(new ItemsRemovedFromInventory(Id, count));
        }

        public void Deactivate()
        {
            if (!_memento.Activated) throw new InvalidOperationException("already deactivated");
            ApplyChange(new InventoryItemDeactivated(Id));
        }

        private void Apply(InventoryItemCreated e)
        {
            _memento = new InventorySnapshot();
            _memento.Id = e.Id;
            _memento.Activated = true;
            _memento.Version++;
        }

        private void Apply(InventoryItemRenamed e)
        {
            _memento.Name = e.NewName;
            _memento.Version++;
        }

        private void Apply(ItemsCheckedInToInventory e)
        {
            _memento.Items += e.Count;
            _memento.Version++;
        }

        private void Apply(ItemsRemovedFromInventory e)
        {
            _memento.Items -= e.Count;
            _memento.Version++;
        }

        private void Apply(InventoryItemDeactivated e)
        {
            _memento.Activated = false;
            _memento.Version++;
        }
    }
}
