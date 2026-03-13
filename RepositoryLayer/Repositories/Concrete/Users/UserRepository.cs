using EntityLayer.Entites.Users;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Abstraction.Users;

namespace RepositoryLayer.Repositories.Concrete.Users;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(SmartWarehouseManagementSystemDbContext context) : base(context)
    {
    }
}
