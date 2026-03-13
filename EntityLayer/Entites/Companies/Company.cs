using EntityLayer.Entites.Common;
using EntityLayer.Entites.Products;
using EntityLayer.Entites.Users;
using EntityLayer.Entites.Warehouses;

namespace EntityLayer.Entites.Companies;

public class Company : IEntity
{
    public Company()
    {
        Warehouses = new List<Warehouse>();
        Products = new List<Product>();
        Users = new List<User>();
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Warehouse> Warehouses { get; set; }
    public IEnumerable<Product> Products { get; set; }
    public IEnumerable<User> Users { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
