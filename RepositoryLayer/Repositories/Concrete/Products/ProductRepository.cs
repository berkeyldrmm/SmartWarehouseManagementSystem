using EntityLayer.Entites.Products;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Abstraction.Products;

namespace RepositoryLayer.Repositories.Concrete.Products;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(SmartWarehouseManagementSystemDbContext context) : base(context)
    {
    }
}
