using EntityLayer.Entites.Products;
using System.Linq.Expressions;

namespace EntityLayer.Dtos.Products;

public static class ProductSelectors
{
    public static Expression<Func<Product, ProductAdminDto>> Admin => p => new ProductAdminDto
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        CompanyId = p.CompanyId,
        IsDeleted = p.IsDeleted,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt
    };

    public static Expression<Func<Product, ProductUserDto>> User => p => new ProductUserDto
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price
    };
}
