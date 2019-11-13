using Es04.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es07.Test.Infrastructure
{
    public abstract class SnapshottableAggregateRoot : AggregateRoot
    {
        public abstract void LoadSnapshot(string data);
        public abstract ISnapshot GetSnapshot();

        public abstract bool ShouldCreateSnapshot();
    }
}
