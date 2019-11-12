using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es03.Test.Infrastructure
{
    public interface IAggregateRoot
    {
        void LoadFromHistory(IEnumerable<object> events);
    }
}
