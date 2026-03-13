using EntityLayer.Dtos.Auth;
using EntityLayer.Dtos.Companies.Requests;
using EntityLayer.Dtos.Products;
using EntityLayer.Dtos.Warehouses.Requests;
using EntityLayer.Entites.Users;
using EntityLayer.Options;
using FluentValidation;
using ManagerLayer.Authentication.Abstract;
using ManagerLayer.Authentication.Concrete;
using ManagerLayer.Services.Abstract.Companies;
using ManagerLayer.Services.Abstract.Products;
using ManagerLayer.Services.Abstract.Users;
using ManagerLayer.Services.Abstract.Warehouses;
using ManagerLayer.Services.Concrete.Companies;
using ManagerLayer.Services.Concrete.Products;
using ManagerLayer.Services.Concrete.Users;
using ManagerLayer.Services.Concrete.Warehouses;
using ManagerLayer.Validation.Auth;
using ManagerLayer.Validation.Companies;
using ManagerLayer.Validation.Products;
using ManagerLayer.Validation.Warehouses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Abstraction.Companies;
using RepositoryLayer.Repositories.Abstraction.Products;
using RepositoryLayer.Repositories.Abstraction.Users;
using RepositoryLayer.Repositories.Abstraction.WarehouseProducts;
using RepositoryLayer.Repositories.Abstraction.Warehouses;
using RepositoryLayer.Repositories.Concrete.Companies;
using RepositoryLayer.Repositories.Concrete.Products;
using RepositoryLayer.Repositories.Concrete.Users;
using RepositoryLayer.Repositories.Concrete.WarehouseProducts;
using RepositoryLayer.Repositories.Concrete.Warehouses;
using RepositoryLayer.UnitOfWorks.Abstraction;
using RepositoryLayer.UnitOfWorks.Concrete;
using SmartWarehouseManagementSystem.Middlewares;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("SqlServer");

builder.Services.AddDbContext<SmartWarehouseManagementSystemDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, EntityLayer.Entites.Roles.Role>(x =>
{
    x.Password.RequireNonAlphanumeric = false;
})
.AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider)
.AddEntityFrameworkStores<SmartWarehouseManagementSystemDbContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IWarehouseProductRepository, WarehouseProductRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

builder.Services.AddScoped<IValidator<UserRegisterDto>, UserRegisterDtoValidator>();
builder.Services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();
builder.Services.AddScoped<IValidator<CreateProductForUserDto>, CreateProductForUserDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateProductDto>, UpdateProductDtoValidator>();
builder.Services.AddScoped<IValidator<AddProductToWarehouseDto>, AddProductToWarehouseDtoValidator>();
builder.Services.AddScoped<IValidator<DecreaseWarehouseStockDto>, DecreaseWarehouseStockDtoValidator>();
builder.Services.AddScoped<IValidator<CreateCompanyDto>, CreateCompanyDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateCompanyDto>, UpdateCompanyDtoValidator>();
builder.Services.AddScoped<IValidator<CreateWarehouseDto>, CreateWarehouseDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateWarehouseDto>, UpdateWarehouseDtoValidator>();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["SecretKey"] ?? throw new InvalidOperationException("Jwt:Secret key not configured"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSection["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.Name
    };
    options.SaveToken = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Warehouse Management System API", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter your valid token in the text input below.\r\n\r\nExample: 'eyJhbGci...'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { securityScheme, new[] { "Bearer" } }
    });
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
