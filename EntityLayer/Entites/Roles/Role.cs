using EntityLayer.Entites.Common;
using EntityLayer.Entites.UserRoles;
using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Entites.Roles;

public class Role : IdentityRole<Guid>, IEntity
{
    public Role()
    {
        Id = Guid.NewGuid();
    }
    public IEnumerable<UserRole> UserRoles { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
