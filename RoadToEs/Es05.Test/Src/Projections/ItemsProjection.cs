using Es01.Test.Src.Events;
using Es02.Test.Src.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es05.Test.Src.Projections
{
    public class ItemsProjection
    {
        private Dictionary<Guid, string> _items = new Dictionary<Guid, string>();
        public ItemsProjection(E05.Test.Infrastructure.Bus bus)
        {
            bus.RegisterTopic<InventoryItemCreated>(Handle);
            bus.RegisterTopic<ItemNameModified>(Handle);
        }

        private void Handle(ItemNameModified @event)
        {
            _items[@event.Id] = @event.NewName;
        }

        private void Handle(InventoryItemCreated @event)
        {
            _items[@event.Id] = @event.Name;
        }

        public IEnumerable<KeyValuePair<Guid,string>> GetAll()
        {
            return _items;
        }
    }
}
