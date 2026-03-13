using EntityLayer.Entites.Companies;
using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Entites.Users;

public class User : IdentityUser<Guid>
{
    public User()
    {
        Id = Guid.NewGuid();
    }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    public bool IsDeleted { get; set; }
}
