using EntityLayer.Entites.Common;
using EntityLayer.Entites.Companies;

namespace EntityLayer.Entites.Products;

public class Product : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
