namespace Crud
{
    public interface IOptimisticEntity:IEntity
    {
        int Version { get; set; }
    }
}
