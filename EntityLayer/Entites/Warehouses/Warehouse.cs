using EntityLayer.Entites.Common;
using EntityLayer.Entites.Companies;

namespace EntityLayer.Entites.Warehouses;
public class Warehouse : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
