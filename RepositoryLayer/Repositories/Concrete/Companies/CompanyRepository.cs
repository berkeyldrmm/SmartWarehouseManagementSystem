using EntityLayer.Entites.Companies;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Abstraction.Companies;
using RepositoryLayer.UnitOfWorks.Abstraction;

namespace RepositoryLayer.Repositories.Concrete.Companies;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(SmartWarehouseManagementSystemDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
