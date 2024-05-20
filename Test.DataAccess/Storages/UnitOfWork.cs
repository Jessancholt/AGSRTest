using Test.DataAccess.Storages.Interfaces;

namespace Test.DataAccess.Storages
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly TestDbContext _context;

        public UnitOfWork(TestDbContext context)
        {
            _context = context;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
