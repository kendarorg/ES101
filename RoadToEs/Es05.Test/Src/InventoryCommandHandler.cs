using E05.Test.Infrastructure;
using Es01.Test.Src.Commands;
using Es02.Test.Infrastructure;
using Es02.Test.Src.Commands;

namespace Es01.Test.Src
{

    public class InventoryCommandHandler
    {
        private readonly EventStore _eventStore;
        private readonly E05.Test.Infrastructure.Bus _bus;

        public InventoryCommandHandler(E05.Test.Infrastructure.Bus bus, EventStore eventStore)
        {
            _eventStore = eventStore;
            _bus = bus;
            _bus.RegisterQueue<CreateInventoryItem>(Handle);
            _bus.RegisterQueue<ModifyItemName>(Handle);
        }

        public void Handle(CreateInventoryItem command)
        {
            var aggregate = new InventoryAggregateRoot(command.Id, command.Name);
            _eventStore.Save(aggregate.Id, aggregate.GetUncommittedChanges(),-1);
        }

        public void Handle(ModifyItemName command)
        {
            var aggregate = _eventStore.GetById<InventoryAggregateRoot>(command.Id);
            aggregate.ChangeName(command.NewName);
            _eventStore.Save(aggregate.Id, aggregate.GetUncommittedChanges(),command.ExpectedVersion);
        }
    }
}
