using EntityLayer.Entites.Common;
using EntityLayer.Entites.Roles;
using EntityLayer.Entites.Users;

namespace EntityLayer.Entites.UserRoles;

public class UserRole : IEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
