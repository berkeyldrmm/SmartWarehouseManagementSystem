using ManagerLayer.Services.Abstract.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ManagerLayer.Authorization;

public class RoleValidationFilter : IAsyncAuthorizationFilter
{
    public string _role;
    private readonly IUserService _userService;

    public RoleValidationFilter(string role, IUserService userService)
    {
        _userService = userService;
        _role = role;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        Claim? userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        bool exists = await _userService.IsInRoleAsync(userIdClaim.Value, _role);
        if (!exists)
        {
            context.Result = new ForbidResult();
        }
    }
}
