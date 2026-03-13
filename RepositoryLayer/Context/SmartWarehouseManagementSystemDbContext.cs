using EntityLayer.Entites.Companies;
using EntityLayer.Entites.Products;
using EntityLayer.Entites.Roles;
using EntityLayer.Entites.UserRoles;
using EntityLayer.Entites.Users;
using EntityLayer.Entites.WarehouseProducts;
using EntityLayer.Entites.Warehouses;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Context;

public class SmartWarehouseManagementSystemDbContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<WarehouseProduct> WarehouseProducts { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
}
