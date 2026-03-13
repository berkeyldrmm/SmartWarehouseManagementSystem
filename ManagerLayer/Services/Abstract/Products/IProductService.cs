using EntityLayer.Dtos.Products;
using EntityLayer.Entites.Products;
using RepositoryLayer.Pagination;
using System.Linq.Expressions;

namespace ManagerLayer.Services.Abstract.Products;

public interface IProductService
{
    Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<Product, TResult>> selector, string? userId, bool isAdmin);
    Task<TResult> GetByIdAsync<TResult>(Guid id, Expression<Func<Product, TResult>> selector, string? userId, bool isAdmin);
    Task<TResult?> FirstOrDefaultAsync<TResult>(ProductFilterDto filter, Expression<Func<Product, TResult>> selector, string? userId, bool isAdmin);
    Task<IEnumerable<TResult>> GetByFiltersAsync<TResult>(ProductFilterDto filter, Expression<Func<Product, TResult>> selector, string? userId, bool isAdmin);
    Task<PagedDataListModel<TResult>> GetPagedDataListAsync<TResult>(int pageNumber, int pageSize, ProductFilterDto filter, Expression<Func<Product, TResult>> selector, string? userId, bool isAdmin);

    Task<int> CountAsync(ProductFilterDto filter, string? userId, bool isAdmin);
    Task<bool> ExistsByIdAsync(Guid id, string? userId, bool isAdmin);
    Task<bool> AddAsync(CreateProductDto product, string? userId, bool isAdmin);
    Task<bool> AddForUserAsync(CreateProductForUserDto product, string userId);
    Task<bool> UpdateAsync(Guid id, UpdateProductDto product, string? userId, bool isAdmin);
    Task<bool> DeleteAsync(Guid id, string? userId, bool isAdmin);
    Task DeleteRangeAsync(IEnumerable<Guid> ids, string? userId, bool isAdmin);
    Task<bool> AddProductToWarehouseAsync(Guid productId, AddProductToWarehouseDto request, string userId);
    Task<bool> RemoveProductFromWarehouseAsync(Guid productId, Guid warehouseId, string userId);
    Task<bool> DecreaseWarehouseStockAsync(Guid productId, DecreaseWarehouseStockDto request, string userId);
    Task<PagedDataListModel<WarehouseProductItemDto>> GetWarehouseProductsForUserAsync(Guid warehouseId, int pageNumber, int pageSize, string userId);
}
