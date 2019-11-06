using Crud;

namespace InMemory.Repository
{
    public class OptimisticEntity :Entity, IOptimisticEntity
    {
        public int Version { get; set; }
    }
}
