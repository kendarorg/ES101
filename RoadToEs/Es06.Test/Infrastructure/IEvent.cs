using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es05.Test.Infrastructure
{
    public interface IEvent
    {
        int Version { get; set; }
    }
}
