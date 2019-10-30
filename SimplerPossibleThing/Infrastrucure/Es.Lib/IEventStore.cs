using System;
using System.Collections.Generic;
using System.Text;

namespace Es.Lib
{
    public interface IEventStore 
    {
        void Save<T>(T root, int version) where T : AggregateRoot;
        T GetById<T>(T newRoot,Guid inventoryItemId) where T : AggregateRoot;
    }
}
