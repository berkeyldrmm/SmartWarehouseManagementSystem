using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Warehouses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "WarehouseProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WarehouseProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "UserRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[] { new Guid("a70d6c2c-2cf9-4a8a-9e6f-3492f4a9c403"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "System Company", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "DeletedAt", "IsDeleted", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("2b63b918-d1d5-45e2-b4f4-3e7d9e89f201"), "D0A3E7A1-FA13-4A1C-9E35-61B14576C9C1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Admin", "ADMIN", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b3a48641-1ab6-4d32-8ca5-f93bb4b5d402"), "A4D6BBAF-77CF-4E53-9CF6-7DE6E70F2A2B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "User", "USER", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "CompanyId", "ConcurrencyStamp", "CreatedAt", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("9d3b6cc1-6f1d-4a80-8ed7-232fce6d9404"), 0, new Guid("a70d6c2c-2cf9-4a8a-9e6f-3492f4a9c403"), "DD21567B-BBC6-4AE2-B8D8-8A365431EA2A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "admin@gmail.com", true, false, false, null, "System", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEChOg53RkXnUzVsF5IlJ2wwZXL7FW3GkWNa8sdDoCAb7cDKpV5eUVC1RySlyeQHDaA==", null, false, "2BAA6C30-8C4A-45F0-83AD-E0D89A0CC51A", "Admin", false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "RoleId", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("63ec6f2d-0d67-4c09-b7f9-1fc1cff84305"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new Guid("2b63b918-d1d5-45e2-b4f4-3e7d9e89f201"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9d3b6cc1-6f1d-4a80-8ed7-232fce6d9404") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b3a48641-1ab6-4d32-8ca5-f93bb4b5d402"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: new Guid("63ec6f2d-0d67-4c09-b7f9-1fc1cff84305"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2b63b918-d1d5-45e2-b4f4-3e7d9e89f201"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9d3b6cc1-6f1d-4a80-8ed7-232fce6d9404"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("a70d6c2c-2cf9-4a8a-9e6f-3492f4a9c403"));

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "WarehouseProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WarehouseProducts");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Companies");
        }
    }
}
