using Es01.Test.Src.Commands;
using System;
using System.Collections.Generic;

namespace Es01.Test.Src
{
    public class EventDescriptor
    {
        public Guid Id { get; set; }
        public object Data { get; set; }
    }
    public class InventoryCommandHandler
    {
        public List<EventDescriptor> Events { get; private set; }
        public InventoryCommandHandler()
        {
            Events = new List<EventDescriptor>();
        }
        protected void Save(Guid id,IEnumerable<object> events)
        {
            foreach(var @event in events)
            {
                Events.Add(new EventDescriptor
                {
                    Id = id,
                    Data = @event
                });
            }
        }

        public void Handle(CreateInventoryItem command)
        {
            var aggregate = new InventoryAggregateRoot(command.Id,command.Name);
            Save(aggregate.Id,aggregate.GetUncommittedChanges());
            aggregate.ClearUncommittedChanges();
            
        }
    }
}
