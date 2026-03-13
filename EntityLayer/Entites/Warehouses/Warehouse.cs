using EntityLayer.Entites.Common;
using EntityLayer.Entites.Companies;

namespace EntityLayer.Entites.Warehouses;
public class Warehouse : BaseEntity
{
    public string Name { get; set; }
    public string Location { get; set; }
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    public bool IsDeleted { get; set; }
}
