using Es01.Test.Src.Events;
using Es02.Test.Src.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es05.Test.Src.Projections
{
    public class ItemsProjectionEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
    }
    public class ItemsProjection
    {
        private readonly Dictionary<Guid, ItemsProjectionEntity> _items = new Dictionary<Guid, ItemsProjectionEntity>();
        public ItemsProjection(E05.Test.Infrastructure.Bus bus)
        {
            bus.RegisterTopic<InventoryItemCreated>(Handle);
            bus.RegisterTopic<ItemNameModified>(Handle);
        }

        private void Handle(ItemNameModified @event)
        {
            if (_items[@event.Id].Version <= @event.Version)
            {
                _items[@event.Id].Name = @event.NewName;
                _items[@event.Id].Version = @event.Version;
            }
        }

        private void Handle(InventoryItemCreated @event)
        {
            _items[@event.Id] = new ItemsProjectionEntity
            {
                Id = @event.Id,
                Version = @event.Version,
                Name = @event.Name
            };

        }

        public IEnumerable<ItemsProjectionEntity> GetAll()
        {
            return _items.Values;
        }

        public ItemsProjectionEntity GetById(Guid id)
        {
            return _items[id];
        }
    }
}
