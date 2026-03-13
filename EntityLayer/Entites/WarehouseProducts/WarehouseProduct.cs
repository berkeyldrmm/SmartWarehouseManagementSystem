using EntityLayer.Entites.Common;
using EntityLayer.Entites.Products;
using EntityLayer.Entites.Warehouses;

namespace EntityLayer.Entites.WarehouseProducts;

public class WarehouseProduct : IEntity
{
    public Guid Id { get; set; }
    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
