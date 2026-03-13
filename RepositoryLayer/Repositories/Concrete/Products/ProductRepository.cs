using EntityLayer.Entites.Products;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Abstraction.Products;
using RepositoryLayer.UnitOfWorks.Abstraction;

namespace RepositoryLayer.Repositories.Concrete.Products;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(SmartWarehouseManagementSystemDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
