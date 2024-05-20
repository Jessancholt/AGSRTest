using System.Linq.Expressions;

namespace Test.DataAccess.Storages.Finders.Interfaces
{
    public interface IFinder<T> where T : class
    {
        Task<List<T>> GetAsync(CancellationToken cancellationToken);

        Task<List<T>> GetAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken);

        Task<T> GetAsync(Guid id, CancellationToken cancellationToken);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken);
    }
}
