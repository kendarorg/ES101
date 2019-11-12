
using Es05.Test.Infrastructure;
using System;

namespace Es01.Test.Src.Events
{
    public class InventoryItemCreated: IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }

        public InventoryItemCreated(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
