
using Crud;
using System;
using System.Collections.Generic;
using System.Text;

namespace Es.Lib
{
    public class SnapshotEntity:IOptimisticEntity
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Data { get; set; }
    }
}
