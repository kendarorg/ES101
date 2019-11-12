using Es01.Test.Src.Commands;
using Es02.Test.Infrastructure;

namespace Es01.Test.Src
{

    public class InventoryCommandHandler
    {
        private readonly EventStore _eventStore;

        public InventoryCommandHandler(EventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Handle(CreateInventoryItem command)
        {
            var aggregate = new InventoryAggregateRoot(command.Id,command.Name);
            _eventStore.Save(aggregate.Id,aggregate.GetUncommittedChanges());
            aggregate.ClearUncommittedChanges();
            
        }
    }
}
