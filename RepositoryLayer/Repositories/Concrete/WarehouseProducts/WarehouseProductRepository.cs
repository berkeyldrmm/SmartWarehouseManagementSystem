using EntityLayer.Entites.WarehouseProducts;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Abstraction.WarehouseProducts;
using RepositoryLayer.UnitOfWorks.Abstraction;

namespace RepositoryLayer.Repositories.Concrete.WarehouseProducts;

public class WarehouseProductRepository : Repository<WarehouseProduct>, IWarehouseProductRepository
{
    public WarehouseProductRepository(SmartWarehouseManagementSystemDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
