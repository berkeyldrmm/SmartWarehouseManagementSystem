using EntityLayer.Entites.Common;
using EntityLayer.Entites.Products;
using EntityLayer.Entites.Warehouses;

namespace EntityLayer.Entites.WarehouseProducts;

public class WarehouseProduct : BaseEntity
{
    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}
