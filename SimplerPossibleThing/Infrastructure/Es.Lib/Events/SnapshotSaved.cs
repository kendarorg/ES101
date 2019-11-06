using Es.Lib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Shared.Events
{
    public class SnapshotSaved:IEvent
    {
        public SnapshotSaved(Guid id, int version)
        {
            Id = id;
            Version = version;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
