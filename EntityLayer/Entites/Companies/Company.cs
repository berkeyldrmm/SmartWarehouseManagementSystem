using EntityLayer.Entites.Common;
using EntityLayer.Entites.Products;
using EntityLayer.Entites.Users;
using EntityLayer.Entites.Warehouses;

namespace EntityLayer.Entites.Companies;

public class Company : BaseEntity
{
    public Company()
    {
        Warehouses = new List<Warehouse>();
        Products = new List<Product>();
        Users = new List<User>();
    }
    public string Name { get; set; }
    public IEnumerable<Warehouse> Warehouses { get; set; }
    public IEnumerable<Product> Products { get; set; }
    public IEnumerable<User> Users { get; set; }
    public bool IsDeleted { get; set; }
}
