using EntityLayer.Entites.Companies;
using EntityLayer.Entites.Products;
using EntityLayer.Entites.Roles;
using EntityLayer.Entites.UserRoles;
using EntityLayer.Entites.Users;
using EntityLayer.Entites.WarehouseProducts;
using EntityLayer.Entites.Warehouses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Context;

public class SmartWarehouseManagementSystemDbContext : IdentityDbContext<User, Role, Guid>
{
    public SmartWarehouseManagementSystemDbContext(DbContextOptions<SmartWarehouseManagementSystemDbContext> options)
        : base(options)
    {

    }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<WarehouseProduct> WarehouseProducts { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
        });

        builder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
        });

        builder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles");

            entity.HasOne(ur => ur.User)
              .WithMany(u => u.UserRoles)
              .HasForeignKey(ur => ur.UserId)
              .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ur => ur.Role)
                  .WithMany(r => r.UserRoles)
                  .HasForeignKey(ur => ur.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Company>(entity =>
        {
            entity.ToTable("Companies");

            entity.HasMany(c => c.Warehouses)
                  .WithOne(w => w.Company)
                  .HasForeignKey(w => w.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.Products)
                  .WithOne(p => p.Company)
                  .HasForeignKey(p => p.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.Users)
                  .WithOne(u => u.Company)
                  .HasForeignKey(u => u.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
        });

        builder.Entity<Warehouse>(entity =>
        {
            entity.ToTable("Warehouses");
        });

        builder.Entity<WarehouseProduct>(entity =>
        {
            entity.ToTable("WarehouseProducts");

            entity.HasOne(wp => wp.Warehouse)
                  .WithMany()
                  .HasForeignKey(wp => wp.WarehouseId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(wp => wp.Product)
                  .WithMany()
                  .HasForeignKey(wp => wp.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        SeedData(builder);

        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserRole<Guid>>();
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();
    }

    private static void SeedData(ModelBuilder builder)
    {
        var adminRoleId = Guid.Parse("2B63B918-D1D5-45E2-B4F4-3E7D9E89F201");
        var userRoleId = Guid.Parse("B3A48641-1AB6-4D32-8CA5-F93BB4B5D402");
        var systemCompanyId = Guid.Parse("A70D6C2C-2CF9-4A8A-9E6F-3492F4A9C403");
        var adminUserId = Guid.Parse("9D3B6CC1-6F1D-4A80-8ED7-232FCE6D9404");
        var adminUserRoleId = Guid.Parse("63EC6F2D-0D67-4C09-B7F9-1FC1CFF84305");
        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var adminRole = new Role
        {
            Id = adminRoleId,
            Name = "Admin",
            NormalizedName = "ADMIN",
            ConcurrencyStamp = "D0A3E7A1-FA13-4A1C-9E35-61B14576C9C1",
            IsDeleted = false,
            DeletedAt = null,
            CreatedAt = seedDate,
            UpdatedAt = seedDate
        };

        var userRole = new Role
        {
            Id = userRoleId,
            Name = "User",
            NormalizedName = "USER",
            ConcurrencyStamp = "A4D6BBAF-77CF-4E53-9CF6-7DE6E70F2A2B",
            IsDeleted = false,
            DeletedAt = null,
            CreatedAt = seedDate,
            UpdatedAt = seedDate
        };

        var systemCompany = new Company
        {
            Id = systemCompanyId,
            Name = "System Company",
            IsDeleted = false,
            DeletedAt = null,
            CreatedAt = seedDate,
            UpdatedAt = seedDate
        };

        var adminUser = new User
        {
            Id = adminUserId,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            EmailConfirmed = true,
            SecurityStamp = "2BAA6C30-8C4A-45F0-83AD-E0D89A0CC51A",
            ConcurrencyStamp = "DD21567B-BBC6-4AE2-B8D8-8A365431EA2A",
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0,
            Name = "System",
            Surname = "Admin",
            CompanyId = systemCompanyId,
            IsDeleted = false,
            DeletedAt = null,
            CreatedAt = seedDate,
            UpdatedAt = seedDate,
            PasswordHash = "AQAAAAIAAYagAAAAEChOg53RkXnUzVsF5IlJ2wwZXL7FW3GkWNa8sdDoCAb7cDKpV5eUVC1RySlyeQHDaA=="
        };

        var adminUserRole = new UserRole
        {
            Id = adminUserRoleId,
            UserId = adminUserId,
            RoleId = adminRoleId,
            IsDeleted = false,
            DeletedAt = null,
            CreatedAt = seedDate,
            UpdatedAt = seedDate
        };

        builder.Entity<Role>().HasData(adminRole, userRole);
        builder.Entity<Company>().HasData(systemCompany);
        builder.Entity<User>().HasData(adminUser);
        builder.Entity<UserRole>().HasData(adminUserRole);
    }
}
