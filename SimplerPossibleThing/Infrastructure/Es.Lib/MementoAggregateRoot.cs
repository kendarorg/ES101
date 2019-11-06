using Inventory.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Es.Lib
{
    public abstract class MementoAggregateRoot:AggregateRoot
    {
        protected abstract object GetData();

        public void CreateMemento()
        {
            ApplyChange(new MementoCreated(Id,JsonConvert.SerializeObject(GetData())));
        }
    }
}
