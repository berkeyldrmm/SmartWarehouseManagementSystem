namespace ManagerLayer.Services.Abstract.Users;

public interface IUserService
{
    Task<bool> IsInRoleAsync(string userId, string roleName);
    Task<Guid?> GetCompanyIdAsync(string userId);
}
