using EntityLayer.Entites.Warehouses;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Abstraction.Warehouses;
using RepositoryLayer.UnitOfWorks.Abstraction;

namespace RepositoryLayer.Repositories.Concrete.Warehouses;

public class WarehouseRepository : Repository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(SmartWarehouseManagementSystemDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
