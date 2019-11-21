using Es01.Test.Src.Events;
using Es02.Test.Src.Events;
using Es04.Test.Infrastructure;
using Es07.Test.Infrastructure;
using Es07.Test.Src;
using Newtonsoft.Json;
using System;

namespace Es01.Test.Src
{
    public class InventoryAggregateRoot : SnapshottableAggregateRoot
    {
        private InventorySnapshot _snapshot;

        public override Guid Id { get { return _snapshot.Id; } protected set { _snapshot.Id = value; } }
        public override int Version { get { return _snapshot.Version; } protected set { _snapshot.Version = value; } }

        public InventoryAggregateRoot(Guid id, string name)
        {
            _snapshot = new InventorySnapshot();
            Version = -1;
            Id = id;
            ApplyChange(new InventoryItemCreated(id, name));
        }

        public InventoryAggregateRoot()
        {
            _snapshot = new InventorySnapshot();
            Version = -1;
        }

        public void ChangeName(string newName)
        {
            ApplyChange(new ItemNameModified(Id, newName));
        }

        private void Apply(InventoryItemCreated evt)
        {
            Id = evt.Id;
        }

        private void Apply(ItemNameModified evt)
        {
            _snapshot.Name = evt.NewName;
        }

        public override void LoadSnapshot(string data)
        {
            _snapshot = JsonConvert.DeserializeObject<InventorySnapshot>(data);
        }

        public override ISnapshot GetSnapshot()
        {
            return _snapshot;
        }

        public override bool ShouldCreateSnapshot()
        {
            return Version % 2 == 0;
        }
    }
}
