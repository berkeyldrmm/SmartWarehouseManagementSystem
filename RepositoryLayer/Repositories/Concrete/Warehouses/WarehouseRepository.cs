using EntityLayer.Entites.Warehouses;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Abstraction.Warehouses;

namespace RepositoryLayer.Repositories.Concrete.Warehouses;

public class WarehouseRepository : Repository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(SmartWarehouseManagementSystemDbContext context) : base(context)
    {
    }
}
