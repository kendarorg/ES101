using Crud;
using System;
using System.Collections.Generic;
using System.Text;

namespace EsInMemory.Lib
{
    public class Event
    {
        public Guid Id { get; set; }
        public long Version { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
    }
}
