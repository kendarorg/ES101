using Bus;
using Crud;
using Inventory.Shared;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Projections
{

    public class InventoryListView
    {
        private IRepository<InventoryItemListDto> _repository;
        private IBus _bus;
        public InventoryListView(IBus bus,IRepository<InventoryItemListDto> repository)
        {
            _repository = repository;
            _bus = bus;
            _bus.RegisterTopic<InventoryItemCreated>(Handle);
            _bus.RegisterTopic<InventoryItemRenamed>(Handle);
            _bus.RegisterTopic<InventoryItemDeactivated>(Handle);
        }

        public void Handle(InventoryItemCreated message)
        {
            _repository.Save(new InventoryItemListDto(message.Id, message.Name));
        }

        public void Handle(InventoryItemRenamed message)
        {
            var item = _repository.GetById(message.Id);
            item.Name = message.NewName;
            _repository.Save(item);
        }

        public void Handle(InventoryItemDeactivated message)
        {
            _repository.Delete(message.Id);
        }
    }
}
