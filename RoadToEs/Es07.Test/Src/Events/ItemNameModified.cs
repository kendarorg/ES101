
using Es05.Test.Infrastructure;
using System;

namespace Es02.Test.Src.Events
{
    public class ItemNameModified : IEvent
    {
        public Guid Id { get; set; }
        public string NewName { get; set; }
        public int Version { get; set; }
        public ItemNameModified(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }

    }
}
