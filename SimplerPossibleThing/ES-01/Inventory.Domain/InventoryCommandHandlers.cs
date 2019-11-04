using Bus;
using Crud;
using Es.Lib;
using Inventory.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain
{
    public class InventoryCommandHandlers
    {
        private readonly IEventStore _repository;
        private readonly IBus _bus;

        public InventoryCommandHandlers(IBus bus,IEventStore repository)
        {
            _repository = repository;
            _bus = bus;
            _bus.RegisterQueue<CreateInventoryItem>(Handle);
            _bus.RegisterQueue<DeactivateInventoryItem>(Handle);
            _bus.RegisterQueue<RemoveItemsFromInventory>(Handle);
            _bus.RegisterQueue<CheckInItemsToInventory>(Handle);
            _bus.RegisterQueue<RenameInventoryItem>(Handle);
        }

        public void Handle(CreateInventoryItem message)
        {
            var item = new InventoryItem(message.InventoryItemId, message.Name);
            _repository.Save(item, -1);
        }

        public void Handle(DeactivateInventoryItem message)
        {
            var item = _repository.GetById(new InventoryItem(), message.InventoryItemId);
            item.Deactivate();
            _repository.Save(item, message.OriginalVersion);
        }

        public void Handle(RemoveItemsFromInventory message)
        {
            var item = _repository.GetById(new InventoryItem(), message.InventoryItemId);
            item.Remove(message.Count);
            _repository.Save(item, message.OriginalVersion);
        }

        public void Handle(CheckInItemsToInventory message)
        {
            var item = _repository.GetById(new InventoryItem(), message.InventoryItemId);
            item.CheckIn(message.Count);
            _repository.Save(item, message.OriginalVersion);
        }

        public void Handle(RenameInventoryItem message)
        {
            var item = _repository.GetById(new InventoryItem(), message.InventoryItemId);
            item.ChangeName(message.NewName);
            _repository.Save(item, message.OriginalVersion);
        }
    }
}
