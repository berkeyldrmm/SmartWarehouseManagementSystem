using EntityLayer.Dtos.Warehouses.Requests;
using EntityLayer.Entites.Warehouses;

namespace ManagerLayer.Services.Abstract.Warehouses;

public interface IWarehouseService
{
    Task<IEnumerable<Warehouse>> GetAllForUserAsync(string userId);
    Task<Warehouse> GetByIdForUserAsync(Guid id, string userId);
    Task<bool> AddForUserAsync(CreateWarehouseDto request, string userId);
    Task<bool> UpdateForUserAsync(Guid id, UpdateWarehouseDto request, string userId);
    Task<bool> DeleteForUserAsync(Guid id, string userId);
}
