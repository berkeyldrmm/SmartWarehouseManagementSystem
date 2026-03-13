using EntityLayer.Dtos.Products;
using EntityLayer.Entites.Products;
using System.Linq.Expressions;

namespace ManagerLayer.Helpers;

public static class ProductServiceHelper
{
    public static string GetRequiredUserId(string? userId, bool isAdmin)
    {
        if (isAdmin)
        {
            return string.Empty;
        }

        return userId ?? throw new InvalidOperationException("UserId is required for non-admin operations.");
    }

    public static IEnumerable<Expression<Func<Product, bool>>> BuildPredicates(ProductFilterDto filter, Guid userCompanyId, bool isAdmin)
    {
        var predicates = new List<Expression<Func<Product, bool>>>();

        if (!isAdmin)
        {
            predicates.Add(p => p.CompanyId == userCompanyId);
        }


        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            predicates.Add(p => p.Name.Contains(filter.Search) || p.Description.Contains(filter.Search));
        }

        if (filter.MinPrice.HasValue)
        {
            predicates.Add(p => p.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            predicates.Add(p => p.Price <= filter.MaxPrice.Value);
        }

        return predicates;
    }
}
