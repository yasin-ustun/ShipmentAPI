namespace ShipmentAPI.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int Add(TEntity entity);
        IEnumerable<TEntity> GetAll();
    }
}
