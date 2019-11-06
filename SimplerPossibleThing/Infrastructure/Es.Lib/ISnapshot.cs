using System;
using System.Collections.Generic;
using System.Text;

namespace Es.Lib
{
    public interface ISnapshot
    {
        Guid Id { get; set; }
        int Version { get; set; }
    }
}
