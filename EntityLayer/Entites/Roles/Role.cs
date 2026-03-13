using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Entites.Roles;

public class Role : IdentityRole<Guid>
{
    public Role()
    {
        Id = Guid.NewGuid();
    }
    public bool IsDeleted { get; set; }
}
