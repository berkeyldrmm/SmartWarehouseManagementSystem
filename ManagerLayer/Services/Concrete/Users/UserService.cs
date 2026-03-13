using EntityLayer.Entites.Users;
using ManagerLayer.Services.Abstract.Users;
using RepositoryLayer.Repositories.Abstraction.Users;
using System.Linq.Expressions;

namespace ManagerLayer.Services.Concrete.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<bool> IsInRoleAsync(string userId, string roleName)
    {
        Expression<Func<User, bool>> exp = user => user.Id.ToString() == userId && user.UserRoles.Any(ur => ur.Role.Name == roleName);
        IEnumerable<Expression<Func<User, bool>>> expList = new List<Expression<Func<User, bool>>> { exp };
        return _userRepository.ExistAsync(expList);
    }

    public async Task<Guid?> GetCompanyIdAsync(string userId)
    {
        if (!Guid.TryParse(userId, out var parsedUserId))
        {
            return null;
        }

        var user = await _userRepository.GetByIdAsync(parsedUserId);
        return user?.CompanyId;
    }
}
