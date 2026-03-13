namespace EntityLayer.Dtos.Products;

public class AddProductToWarehouseDto
{
    public Guid WarehouseId { get; set; }
    public int Quantity { get; set; }
}
