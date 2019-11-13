using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es07.Test.Infrastructure
{
    public interface ISnapshot
    {
        Guid Id { get; set; }
        int Version { get; set; }
    }
}
