using Bus;
using InMemory.Crud;
using Inventory.Projections;

namespace CQRSGui
{
    public static class ServiceLocator
    {
        public static IBus Bus { get; set; }
        public static InMemoryRepository<InventoryItemDetailsDto> InventoryItemDetailViewRepo { get; internal set; }
        public static InMemoryRepository<InventoryItemListDto> InventoryItemListRepo { get; internal set; }
    }
}