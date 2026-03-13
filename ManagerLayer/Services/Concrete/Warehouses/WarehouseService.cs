using EntityLayer.Dtos.Warehouses.Requests;
using EntityLayer.Entites.Warehouses;
using FluentValidation;
using ManagerLayer.Services.Abstract.Companies;
using ManagerLayer.Services.Abstract.Warehouses;
using RepositoryLayer.Repositories.Abstraction.Warehouses;
using System.Linq.Expressions;

namespace ManagerLayer.Services.Concrete.Warehouses;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly ICompanyService _companyService;
    private readonly IValidator<CreateWarehouseDto> _createWarehouseValidator;
    private readonly IValidator<UpdateWarehouseDto> _updateWarehouseValidator;

    public WarehouseService(
        IWarehouseRepository warehouseRepository,
        ICompanyService companyService,
        IValidator<CreateWarehouseDto> createWarehouseValidator,
        IValidator<UpdateWarehouseDto> updateWarehouseValidator)
    {
        _warehouseRepository = warehouseRepository;
        _companyService = companyService;
        _createWarehouseValidator = createWarehouseValidator;
        _updateWarehouseValidator = updateWarehouseValidator;
    }

    public async Task<IEnumerable<Warehouse>> GetAllForUserAsync(string userId)
    {
        var companyId = await _companyService.GetCompanyIdByUserIdAsync(userId);
        IEnumerable<Expression<Func<Warehouse, bool>>> predicates = [w => w.CompanyId == companyId, w => !w.IsDeleted];
        return await _warehouseRepository.GetByFiltersAsync(predicates);
    }

    public async Task<Warehouse> GetByIdForUserAsync(Guid id, string userId)
    {
        var companyId = await _companyService.GetCompanyIdByUserIdAsync(userId);
        IEnumerable<Expression<Func<Warehouse, bool>>> predicates = [w => w.Id == id, w => w.CompanyId == companyId, w => !w.IsDeleted];
        return await _warehouseRepository.FirstOrDefault(predicates)
            ?? throw new KeyNotFoundException($"Warehouse with id '{id}' was not found.");
    }

    public async Task<bool> AddForUserAsync(CreateWarehouseDto request, string userId)
    {
        await _createWarehouseValidator.ValidateAndThrowAsync(request);

        var companyId = await _companyService.GetCompanyIdByUserIdAsync(userId);
        var entity = new Warehouse
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Location = request.Location,
            CompanyId = companyId,
            IsDeleted = false,
            DeletedAt = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _warehouseRepository.AddAsync(entity);
    }

    public async Task<bool> UpdateForUserAsync(Guid id, UpdateWarehouseDto request, string userId)
    {
        await _updateWarehouseValidator.ValidateAndThrowAsync(request);

        var entity = await GetByIdForUserAsync(id, userId);
        entity.Name = request.Name;
        entity.Location = request.Location;

        return await _warehouseRepository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteForUserAsync(Guid id, string userId)
    {
        var entity = await GetByIdForUserAsync(id, userId);
        return await _warehouseRepository.DeleteAsync(entity);
    }
}
