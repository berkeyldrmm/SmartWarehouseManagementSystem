using EntityLayer.Entites.Companies;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Abstraction.Companies;

namespace RepositoryLayer.Repositories.Concrete.Companies;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(SmartWarehouseManagementSystemDbContext context) : base(context)
    {
    }
}
