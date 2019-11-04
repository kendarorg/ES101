using Bus;
using Crud;
using Inventory.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Projections
{
    public class InventoryItemDetails
    {
        private IRepository<InventoryItemDetailsDto> _repository;
        private IBus _bus;

        public InventoryItemDetails(IBus bus,IRepository<InventoryItemDetailsDto> repository)
        {
            _repository = repository;
            _bus = bus;
            _bus.RegisterTopic<InventoryItemCreated>(Handle);
            _bus.RegisterTopic<InventoryItemRenamed>(Handle);
            _bus.RegisterTopic<InventoryItemDeactivated>(Handle);
            _bus.RegisterTopic<ItemsRemovedFromInventory>(Handle);
            _bus.RegisterTopic<ItemsCheckedInToInventory>(Handle);
        }

        public void Handle(InventoryItemCreated message)
        {
            _repository.Save( new InventoryItemDetailsDto(message.Id, message.Name, 0, 0));
        }

        public void Handle(InventoryItemRenamed message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.Id);
            d.Name = message.NewName;
            d.Version = message.Version;
            _repository.Save(d);
        }

        private InventoryItemDetailsDto GetDetailsItem(Guid id)
        {
            InventoryItemDetailsDto d = _repository.GetById(id);

            if (d==null)
            {
                throw new InvalidOperationException("did not find the original inventory this shouldnt happen");
            }

            return d;
        }

        public void Handle(ItemsRemovedFromInventory message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.Id);
            d.CurrentCount -= message.Count;
            d.Version = message.Version;
            _repository.Save(d);
        }

        public void Handle(ItemsCheckedInToInventory message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.Id);
            d.CurrentCount += message.Count;
            d.Version = message.Version;
            _repository.Save(d);
        }

        public void Handle(InventoryItemDeactivated message)
        {
            _repository.Delete(message.Id);
        }
    }
}
