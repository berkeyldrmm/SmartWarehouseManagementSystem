using EntityLayer.Entites.Common;
using EntityLayer.Entites.Roles;
using EntityLayer.Entites.Users;

namespace EntityLayer.Entites.UserRoles;

public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
}
