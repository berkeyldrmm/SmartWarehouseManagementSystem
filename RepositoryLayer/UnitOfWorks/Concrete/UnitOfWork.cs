using RepositoryLayer.Context;
using RepositoryLayer.UnitOfWorks.Abstraction;

namespace RepositoryLayer.UnitOfWorks.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly SmartWarehouseManagementSystemDbContext _context;

    public UnitOfWork(SmartWarehouseManagementSystemDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
