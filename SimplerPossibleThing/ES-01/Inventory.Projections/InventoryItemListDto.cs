using Crud;
using System;

namespace Inventory.Projections
{
    public class InventoryItemListDto: IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public InventoryItemListDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
