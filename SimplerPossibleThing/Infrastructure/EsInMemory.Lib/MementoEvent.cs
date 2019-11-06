using Crud;
using System;
using System.Collections.Generic;
using System.Text;

namespace EsInMemory.Lib
{
    internal class MementoEvent:Event
    {
        public long LastMementoVersion { get; set; }
    }
}
