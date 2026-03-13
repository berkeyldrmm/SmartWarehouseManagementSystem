using EntityLayer.Entites.Products;

namespace EntityLayer.Dtos.Products;

public static class ProductMappings
{
    public static Product ToEntity(this CreateProductDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CompanyId = dto.CompanyId,
        };
    }

    public static Product ToEntity(this CreateProductForUserDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
        };
    }

    public static void MapToEntity(this UpdateProductDto dto, Product entity)
    {
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Price = dto.Price;
    }
}
