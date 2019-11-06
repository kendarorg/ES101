using Es.Lib;
using System;

namespace Inventory.Shared
{
    public class MementoCreated : IEvent
    {

        public int Version { get; set; }
        public Guid Id { get; set; }
        public string Data { get; set; }

        public MementoCreated(Guid id,string data)
        {
            Id = id;
            Data = data;
        }
    }
}
