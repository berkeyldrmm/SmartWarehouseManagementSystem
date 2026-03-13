using EntityLayer.Entites.Common;
using EntityLayer.Entites.Companies;
using EntityLayer.Entites.UserRoles;
using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Entites.Users;

public class User : IdentityUser<Guid>, IEntity
{
    public User()
    {
        Id = Guid.NewGuid();
    }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    public IEnumerable<UserRole> UserRoles { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
