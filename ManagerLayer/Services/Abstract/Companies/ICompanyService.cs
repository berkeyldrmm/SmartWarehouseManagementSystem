using EntityLayer.Dtos.Companies.Requests;
using EntityLayer.Entites.Companies;

namespace ManagerLayer.Services.Abstract.Companies;

public interface ICompanyService
{
    Task<Guid> GetCompanyIdByUserIdAsync(string userId);
    Task CheckCompanyExistsAsync(Guid companyId);

    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company> GetByIdAsync(Guid id);
    Task<bool> AddAsync(CreateCompanyDto request);
    Task<bool> UpdateAsync(Guid id, UpdateCompanyDto request);
    Task<bool> DeleteAsync(Guid id);
}
