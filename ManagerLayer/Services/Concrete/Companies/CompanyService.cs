using EntityLayer.Dtos.Companies.Requests;
using EntityLayer.Entites.Companies;
using EntityLayer.Entites.Users;
using FluentValidation;
using ManagerLayer.Services.Abstract.Companies;
using RepositoryLayer.Repositories.Abstraction.Companies;
using RepositoryLayer.Repositories.Abstraction.Users;
using System.Linq.Expressions;

namespace ManagerLayer.Services.Concrete.Companies;

public class CompanyService : ICompanyService
{
    private readonly IUserRepository _userRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IValidator<CreateCompanyDto> _createCompanyValidator;
    private readonly IValidator<UpdateCompanyDto> _updateCompanyValidator;

    public CompanyService(
        IUserRepository userRepository,
        ICompanyRepository companyRepository,
        IValidator<CreateCompanyDto> createCompanyValidator,
        IValidator<UpdateCompanyDto> updateCompanyValidator)
    {
        _userRepository = userRepository;
        _companyRepository = companyRepository;
        _createCompanyValidator = createCompanyValidator;
        _updateCompanyValidator = updateCompanyValidator;
    }

    public async Task<Guid> GetCompanyIdByUserIdAsync(string userId)
    {
        if (!Guid.TryParse(userId, out var parsedUserId))
        {
            throw new InvalidOperationException("User id is invalid.");
        }

        IEnumerable<Expression<Func<User, bool>>> predicates = [u => u.Id == parsedUserId];
        var companyId = await _userRepository.FirstOrDefault(predicates, u => (Guid?)u.CompanyId);

        return companyId ?? throw new KeyNotFoundException("Company id could not be resolved for current user.");
    }

    public async Task CheckCompanyExistsAsync(Guid companyId)
    {
        IEnumerable<Expression<Func<Company, bool>>> predicates = [c => c.Id == companyId, c => !c.IsDeleted];
        var exists = await _companyRepository.ExistAsync(predicates);
        if (!exists)
        {
            throw new KeyNotFoundException("Belirtilen company bulunamadý.");
        }
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        IEnumerable<Expression<Func<Company, bool>>> predicates = [c => !c.IsDeleted];
        return await _companyRepository.GetByFiltersAsync(predicates);
    }

    public async Task<Company> GetByIdAsync(Guid id)
    {
        IEnumerable<Expression<Func<Company, bool>>> predicates = [c => c.Id == id, c => !c.IsDeleted];
        return await _companyRepository.FirstOrDefault(predicates)
            ?? throw new KeyNotFoundException($"Company with id '{id}' was not found.");
    }

    public async Task<bool> AddAsync(CreateCompanyDto request)
    {
        await _createCompanyValidator.ValidateAndThrowAsync(request);

        var entity = new Company
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            IsDeleted = false,
            DeletedAt = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _companyRepository.AddAsync(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateCompanyDto request)
    {
        await _updateCompanyValidator.ValidateAndThrowAsync(request);

        var company = await GetByIdAsync(id);
        company.Name = request.Name;

        return await _companyRepository.UpdateAsync(company);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var company = await GetByIdAsync(id);
        return await _companyRepository.DeleteAsync(company);
    }
}
