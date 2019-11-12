using Es01.Test.Src.Commands;
using Es02.Test.Infrastructure;
using Es02.Test.Src.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var aggregate = new InventoryAggregateRoot(command.Id, command.Name);
            _eventStore.Save(aggregate.Id, aggregate.GetUncommittedChanges(),-1);
            aggregate.ClearUncommittedChanges();

        }

        public void Handle(ModifyItemName command)
        {
            var aggregate = _eventStore.GetById<InventoryAggregateRoot>(command.Id);
            aggregate.ChangeName(command.NewName);
            _eventStore.Save(aggregate.Id, aggregate.GetUncommittedChanges(),command.ExpectedVersion);
            aggregate.ClearUncommittedChanges();

        }
    }
}
