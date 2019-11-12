using System.Collections.Generic;

namespace Es03.Test.Infrastructure
{
    public interface IAggregateRoot
    {
        void LoadFromHistory(IEnumerable<object> events);
    }
}
