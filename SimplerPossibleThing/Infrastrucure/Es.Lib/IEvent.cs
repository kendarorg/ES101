using System;
using System.Collections.Generic;
using System.Text;

namespace Es.Lib
{
    public interface IEvent
    {
        int Version { get; set; }
        Guid Id { get; set; }
    }
}
