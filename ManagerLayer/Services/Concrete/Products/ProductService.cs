using EntityLayer.Dtos.Products;
using EntityLayer.Entites.Products;
using EntityLayer.Entites.WarehouseProducts;
using EntityLayer.Entites.Warehouses;
using FluentValidation;
using ManagerLayer.Helpers;
using ManagerLayer.Services.Abstract.Companies;
using ManagerLayer.Services.Abstract.Products;
using RepositoryLayer.Pagination;
using RepositoryLayer.Repositories.Abstraction.Products;
using RepositoryLayer.Repositories.Abstraction.WarehouseProducts;
using RepositoryLayer.Repositories.Abstraction.Warehouses;
using System.Linq.Expressions;

namespace ManagerLayer.Services.Concrete.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IWarehouseProductRepository _warehouseProductRepository;
    private readonly ICompanyService _companyService;
    private readonly IValidator<CreateProductDto> _createProductValidator;
    private readonly IValidator<CreateProductForUserDto> _createProductForUserValidator;
    private readonly IValidator<UpdateProductDto> _updateProductValidator;
    private readonly IValidator<AddProductToWarehouseDto> _addProductToWarehouseValidator;
    private readonly IValidator<DecreaseWarehouseStockDto> _decreaseWarehouseStockValidator;

    public ProductService(
        IProductRepository productRepository,
        IWarehouseRepository warehouseRepository,
        IWarehouseProductRepository warehouseProductRepository,
        ICompanyService companyService,
        IValidator<CreateProductDto> createProductValidator,
        IValidator<CreateProductForUserDto> createProductForUserValidator,
        IValidator<UpdateProductDto> updateProductValidator,
        IValidator<AddProductToWarehouseDto> addProductToWarehouseValidator,
        IValidator<DecreaseWarehouseStockDto> decreaseWarehouseStockValidator)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _warehouseProductRepository = warehouseProductRepository;
        _companyService = companyService;
        _createProductValidator = createProductValidator;
        _createProductForUserValidator = createProductForUserValidator;
        _updateProductValidator = updateProductValidator;
        _addProductToWarehouseValidator = addProductToWarehouseValidator;
        _decreaseWarehouseStockValidator = decreaseWarehouseStockValidator;
    }

    public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<Product, TResult>> selector, string? userId, bool isAdmin)
    {
        if (isAdmin)
        {
            return await _productRepository.GetAllAsync(selector);
        }

        var requiredUserId = ProductServiceHelper.GetRequiredUserId(userId, isAdmin);
        var userCompanyId = await _companyService.GetCompanyIdByUserIdAsync(requiredUserId);
        var predicates = ProductServiceHelper.BuildPredicates(new ProductFilterDto(), userCompanyId, false);
        return await _productRepository.GetByFiltersAsync(predicates, selector);
    }

    public async Task<TResult> GetByIdAsync<TResult>(Guid id, Expression<Func<Product, TResult>> selector, string? userId, bool isAdmin)
    {
        if (isAdmin)
        {
            return await _productRepository.GetByIdAsync(id, selector);
        }

        var requiredUserId = ProductServiceHelper.GetRequiredUserId(userId, isAdmin);
        var userCompanyId = await _companyService.GetCompanyIdByUserIdAsync(requiredUserId);

        IEnumerable<Expression<Func<Product, bool>>> predicates =
        [
            p => p.Id == id,
            p => p.CompanyId == userCompanyId
        ];

        var projected = await _productRepository.FirstOrDefault(predicates, selector);
        if (projected is null)
        {
            throw new KeyNotFoundException($"Product with id '{id}' was not found.");
        }

        return projected;
    }

    public async Task<TResult?> FirstOrDefaultAsync<TResult>(ProductFilterDto filter, Expression<Func<Product, TResult>> selector, string? userId, bool isAdmin)
    {
        var userCompanyId = isAdmin ? Guid.Empty : await _companyService.GetCompanyIdByUserIdAsync(ProductServiceHelper.GetRequiredUserId(userId, isAdmin));
        var predicates = ProductServiceHelper.BuildPredicates(filter, userCompanyId, isAdmin);
        return await _productRepository.FirstOrDefault(predicates, selector);
    }

    public async Task<IEnumerable<TResult>> GetByFiltersAsync<TResult>(ProductFilterDto filter, Expression<Func<Product, TResult>> selector, string? userId, bool isAdmin)
    {
        var userCompanyId = isAdmin ? Guid.Empty : await _companyService.GetCompanyIdByUserIdAsync(ProductServiceHelper.GetRequiredUserId(userId, isAdmin));
        var predicates = ProductServiceHelper.BuildPredicates(filter, userCompanyId, isAdmin);
        return await _productRepository.GetByFiltersAsync(predicates, selector);
    }

    public async Task<PagedDataListModel<TResult>> GetPagedDataListAsync<TResult>(int pageNumber, int pageSize, ProductFilterDto filter, Expression<Func<Product, TResult>> selector, string? userId, bool isAdmin)
    {
        var userCompanyId = isAdmin ? Guid.Empty : await _companyService.GetCompanyIdByUserIdAsync(ProductServiceHelper.GetRequiredUserId(userId, isAdmin));
        var predicates = ProductServiceHelper.BuildPredicates(filter, userCompanyId, isAdmin);
        return await _productRepository.GetPagedDataListAsync(pageNumber, pageSize, selector, predicates);
    }

    public async Task<int> CountAsync(ProductFilterDto filter, string? userId, bool isAdmin)
    {
        var userCompanyId = isAdmin ? Guid.Empty : await _companyService.GetCompanyIdByUserIdAsync(ProductServiceHelper.GetRequiredUserId(userId, isAdmin));
        var predicates = ProductServiceHelper.BuildPredicates(filter, userCompanyId, isAdmin);
        return await _productRepository.CountAsync(predicates);
    }

    public async Task<bool> ExistsByIdAsync(Guid id, string? userId, bool isAdmin)
    {
        if (isAdmin)
        {
            IEnumerable<Expression<Func<Product, bool>>> adminPredicates = [p => p.Id == id];
            return await _productRepository.ExistAsync(adminPredicates);
        }

        var requiredUserId = ProductServiceHelper.GetRequiredUserId(userId, isAdmin);
        var userCompanyId = await _companyService.GetCompanyIdByUserIdAsync(requiredUserId);
        IEnumerable<Expression<Func<Product, bool>>> predicates =
        [
            p => p.Id == id,
            p => p.CompanyId == userCompanyId
        ];

        return await _productRepository.ExistAsync(predicates);
    }

    public async Task<bool> AddAsync(CreateProductDto product, string? userId, bool isAdmin)
    {
        await _createProductValidator.ValidateAndThrowAsync(product);

        var entity = product.ToEntity();

        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }

        if (!isAdmin)
        {
            throw new InvalidOperationException("Non-admin product creation should use AddForUserAsync.");
        }

        return await _productRepository.AddAsync(entity);
    }

    public async Task<bool> AddForUserAsync(CreateProductForUserDto product, string userId)
    {
        await _createProductForUserValidator.ValidateAndThrowAsync(product);

        var entity = product.ToEntity();
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }

        var userCompanyId = await _companyService.GetCompanyIdByUserIdAsync(userId);
        entity.CompanyId = userCompanyId;

        return await _productRepository.AddAsync(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateProductDto product, string? userId, bool isAdmin)
    {
        await _updateProductValidator.ValidateAndThrowAsync(product);

        Product existingProduct;
        if (isAdmin)
        {
            existingProduct = await _productRepository.GetByIdAsync(id);
        }
        else
        {
            var requiredUserId = ProductServiceHelper.GetRequiredUserId(userId, isAdmin);
            var userCompanyId = await _companyService.GetCompanyIdByUserIdAsync(requiredUserId);
            IEnumerable<Expression<Func<Product, bool>>> predicates =
            [
                p => p.Id == id,
                p => p.CompanyId == userCompanyId
            ];

            existingProduct = await _productRepository.FirstOrDefault(predicates)
                ?? throw new KeyNotFoundException($"Product with id '{id}' was not found.");

            existingProduct.CompanyId = userCompanyId;
        }

        product.MapToEntity(existingProduct);

        return await _productRepository.UpdateAsync(existingProduct);
    }

    public async Task<bool> DeleteAsync(Guid id, string? userId, bool isAdmin)
    {
        Product product;
        if (isAdmin)
        {
            product = await _productRepository.GetByIdAsync(id);
        }
        else
        {
            var requiredUserId = ProductServiceHelper.GetRequiredUserId(userId, isAdmin);
            var userCompanyId = await _companyService.GetCompanyIdByUserIdAsync(requiredUserId);
            IEnumerable<Expression<Func<Product, bool>>> predicates =
            [
                p => p.Id == id,
                p => p.CompanyId == userCompanyId
            ];

            product = await _productRepository.FirstOrDefault(predicates)
                ?? throw new KeyNotFoundException($"Product with id '{id}' was not found.");
        }

        return await _productRepository.DeleteAsync(product);
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<Guid> ids, string? userId, bool isAdmin)
    {
        if (isAdmin)
        {
            return await _productRepository.DeleteRangeAsync(ids);
        }

        var requiredUserId = ProductServiceHelper.GetRequiredUserId(userId, isAdmin);
        var userCompanyId = await _companyService.GetCompanyIdByUserIdAsync(requiredUserId);

        IEnumerable<Expression<Func<Product, bool>>> predicates =
        [
            p => ids.Contains(p.Id),
            p => p.CompanyId == userCompanyId
        ];

        var allowedIds = await _productRepository.GetByFiltersAsync(predicates, p => p.Id);
        return await _productRepository.DeleteRangeAsync(allowedIds);
    }

    public async Task<bool> AddProductToWarehouseAsync(Guid productId, AddProductToWarehouseDto request, string userId)
    {
        await _addProductToWarehouseValidator.ValidateAndThrowAsync(request);

        var userCompanyId = await _companyService.GetCompanyIdByUserIdAsync(userId);

        IEnumerable<Expression<Func<Product, bool>>> productPredicates =
        [
            p => p.Id == productId,
            p => p.CompanyId == userCompanyId,
            p => !p.IsDeleted
        ];

        var productExists = await _productRepository.ExistAsync(productPredicates);
        if (!productExists)
        {
            throw new KeyNotFoundException("Ürün bulunamadý.");
        }

        IEnumerable<Expression<Func<Warehouse, bool>>> warehousePredicates =
        [
            w => w.Id == request.WarehouseId,
            w => w.CompanyId == userCompanyId,
            w => !w.IsDeleted
        ];

        var warehouseExists = await _warehouseRepository.ExistAsync(warehousePredicates);
        if (!warehouseExists)
        {
            throw new KeyNotFoundException("Depo bulunamadý.");
        }

        IEnumerable<Expression<Func<WarehouseProduct, bool>>> warehouseProductPredicates =
        [
            wp => wp.ProductId == productId,
            wp => wp.WarehouseId == request.WarehouseId
        ];

        var warehouseProduct = await _warehouseProductRepository.FirstOrDefault(warehouseProductPredicates);

        if (warehouseProduct is null)
        {
            return await _warehouseProductRepository.AddAsync(new WarehouseProduct
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                WarehouseId = request.WarehouseId,
                Quantity = request.Quantity,
                IsDeleted = false,
                DeletedAt = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        warehouseProduct.Quantity += request.Quantity;
        warehouseProduct.IsDeleted = false;
        warehouseProduct.DeletedAt = null;
        warehouseProduct.UpdatedAt = DateTime.UtcNow;

        return await _warehouseProductRepository.UpdateAsync(warehouseProduct);
    }

    public async Task<bool> RemoveProductFromWarehouseAsync(Guid productId, Guid warehouseId, string userId)
    {
        var userCompanyId = await _companyService.GetCompanyIdByUserIdAsync(userId);

        IEnumerable<Expression<Func<Product, bool>>> productPredicates =
        [
            p => p.Id == productId,
            p => p.CompanyId == userCompanyId,
            p => !p.IsDeleted
        ];

        var productExists = await _productRepository.ExistAsync(productPredicates);

        IEnumerable<Expression<Func<Warehouse, bool>>> warehousePredicates =
        [
            w => w.Id == warehouseId,
            w => w.CompanyId == userCompanyId,
            w => !w.IsDeleted
        ];

        var warehouseExists = await _warehouseRepository.ExistAsync(warehousePredicates);

        if (!productExists || !warehouseExists)
        {
            throw new KeyNotFoundException("Ürün-depo kaydý bulunamadý.");
        }

        IEnumerable<Expression<Func<WarehouseProduct, bool>>> warehouseProductPredicates =
        [
            wp => wp.ProductId == productId,
            wp => wp.WarehouseId == warehouseId,
            wp => !wp.IsDeleted
        ];

        var warehouseProduct = await _warehouseProductRepository.FirstOrDefault(warehouseProductPredicates)
            ?? throw new KeyNotFoundException("Ürün-depo kaydý bulunamadý.");

        warehouseProduct.Quantity = 0;
        warehouseProduct.IsDeleted = true;
        warehouseProduct.DeletedAt = DateTime.UtcNow;
        warehouseProduct.UpdatedAt = DateTime.UtcNow;

        return await _warehouseProductRepository.UpdateAsync(warehouseProduct);
    }

    public async Task<bool> DecreaseWarehouseStockAsync(Guid productId, DecreaseWarehouseStockDto request, string userId)
    {
        await _decreaseWarehouseStockValidator.ValidateAndThrowAsync(request);

        var userCompanyId = await _companyService.GetCompanyIdByUserIdAsync(userId);

        IEnumerable<Expression<Func<Product, bool>>> productPredicates =
        [
            p => p.Id == productId,
            p => p.CompanyId == userCompanyId,
            p => !p.IsDeleted
        ];

        var productExists = await _productRepository.ExistAsync(productPredicates);

        IEnumerable<Expression<Func<Warehouse, bool>>> warehousePredicates =
        [
            w => w.Id == request.WarehouseId,
            w => w.CompanyId == userCompanyId,
            w => !w.IsDeleted
        ];

        var warehouseExists = await _warehouseRepository.ExistAsync(warehousePredicates);

        if (!productExists || !warehouseExists)
        {
            throw new KeyNotFoundException("Ürün-depo kaydý bulunamadý.");
        }

        IEnumerable<Expression<Func<WarehouseProduct, bool>>> warehouseProductPredicates =
        [
            wp => wp.ProductId == productId,
            wp => wp.WarehouseId == request.WarehouseId,
            wp => !wp.IsDeleted
        ];

        var warehouseProduct = await _warehouseProductRepository.FirstOrDefault(warehouseProductPredicates)
            ?? throw new KeyNotFoundException("Ürün-depo kaydý bulunamadý.");

        if (warehouseProduct.Quantity < request.Quantity)
        {
            return false;
        }

        warehouseProduct.Quantity -= request.Quantity;
        warehouseProduct.UpdatedAt = DateTime.UtcNow;

        if (warehouseProduct.Quantity == 0)
        {
            warehouseProduct.IsDeleted = true;
            warehouseProduct.DeletedAt = DateTime.UtcNow;
        }

        return await _warehouseProductRepository.UpdateAsync(warehouseProduct);
    }
}
