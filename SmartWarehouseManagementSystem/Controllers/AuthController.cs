using EntityLayer.Dtos.Auth;
using EntityLayer.Entites.Roles;
using EntityLayer.Entites.UserRoles;
using EntityLayer.Entites.Users;
using FluentValidation;
using ManagerLayer.Authentication.Abstract;
using ManagerLayer.Authentication.Concrete;
using ManagerLayer.Authorization;
using ManagerLayer.Services.Abstract.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using RepositoryLayer.UnitOfWorks.Abstraction;
using SmartWarehouseManagementSystem.Wrappers;

namespace SmartWarehouseManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly SmartWarehouseManagementSystemDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyService _companyService;
    private readonly IValidator<UserRegisterDto> _userRegisterValidator;

    public AuthController(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IJwtProvider jwtProvider,
        SmartWarehouseManagementSystemDbContext context,
        IUnitOfWork unitOfWork,
        ICompanyService companyService,
        IValidator<UserRegisterDto> userRegisterValidator)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtProvider = jwtProvider;
        _context = context;
        _unitOfWork = unitOfWork;
        _companyService = companyService;
        _userRegisterValidator = userRegisterValidator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var user = await _userManager.FindByNameAsync(request.UserNameOrEmail)
            ?? await _userManager.FindByEmailAsync(request.UserNameOrEmail);

        if (user is null)
        {
            return Unauthorized(new ErrorResponse(401, ["Kullanýcý adý/email veya þifre hatalý."]));
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            return Unauthorized(new ErrorResponse(401, ["Kullanýcý adý/email veya þifre hatalý."]));
        }

        var token = _jwtProvider.CreateToken(user);
        return Ok(new DataResponse<LoginResponse>(token));
    }

    [HttpPost("user-register")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> UserRegister([FromBody] UserRegisterDto request)
    {
        var validationResult = await _userRegisterValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse(400, validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        await _companyService.CheckCompanyExistsAsync(request.CompanyId);

        var user = new User
        {
            Name = request.Name,
            Surname = request.Surname,
            UserName = request.UserName,
            Email = request.Email,
            CompanyId = request.CompanyId,
            IsDeleted = false,
            DeletedAt = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            return BadRequest(new ErrorResponse(400, createResult.Errors.Select(e => e.Description)));
        }

        var userRole = await _roleManager.FindByNameAsync("User");
        if (userRole is null)
        {
            return BadRequest(new ErrorResponse(400, ["User rolü bulunamadý."]));
        }

        await _context.UserRoles.AddAsync(new UserRole
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            RoleId = userRole.Id,
            IsDeleted = false,
            DeletedAt = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        await _unitOfWork.SaveChangesAsync();

        return Ok(new DataResponse<Guid>(user.Id));
    }
}
