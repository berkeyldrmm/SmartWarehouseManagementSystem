using EntityLayer.Entites.WarehouseProducts;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Abstraction.WarehouseProducts;

namespace RepositoryLayer.Repositories.Concrete.WarehouseProducts;

public class WarehouseProductRepository : Repository<WarehouseProduct>, IWarehouseProductRepository
{
    public WarehouseProductRepository(SmartWarehouseManagementSystemDbContext context) : base(context)
    {
    }
}
