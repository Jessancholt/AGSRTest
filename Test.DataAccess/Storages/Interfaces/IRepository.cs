namespace Test.DataAccess.Storages.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken);

        Task CreateAsync(List<T> entities, CancellationToken cancellationToken);

        T Update(T entity);

        T Delete(T entity);
    }
}
