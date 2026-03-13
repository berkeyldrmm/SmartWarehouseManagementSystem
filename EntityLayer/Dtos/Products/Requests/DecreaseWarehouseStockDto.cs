namespace EntityLayer.Dtos.Products;

public class DecreaseWarehouseStockDto
{
    public Guid WarehouseId { get; set; }
    public int Quantity { get; set; }
}
