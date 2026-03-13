using EntityLayer.Entites.Users;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Abstraction.Users;
using RepositoryLayer.UnitOfWorks.Abstraction;

namespace RepositoryLayer.Repositories.Concrete.Users;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(SmartWarehouseManagementSystemDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
