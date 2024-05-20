using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Test.DataAccess.Storages.Finders.Interfaces;

namespace Test.DataAccess.Storages.Finders
{
    internal sealed class Finder<T> : IFinder<T> 
        where T : class
    {
        private readonly TestDbContext _context;

        public Finder(TestDbContext context)
        {
            _context = context;
        }

        public Task<List<T>> GetAsync(CancellationToken cancellationToken)
        {
            return Find().ToListAsync(cancellationToken);
        }

        public Task<List<T>> GetAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken)
        {
            return Find().Where(condition).ToListAsync(cancellationToken);
        }

        public Task<T> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return Find(id, cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken)
        {
            return Find().FirstOrDefaultAsync(condition, cancellationToken);
        }

        private IQueryable<T> Find()
        {
            return _context.Set<T>();
        }

        private Task<T> Find(Guid id, CancellationToken cancellationToken)
        {
            return _context.FindAsync<T>(id, cancellationToken).AsTask();
        }
    }
}
