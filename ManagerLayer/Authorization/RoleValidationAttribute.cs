using ManagerLayer.Services.Abstract.Users;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ManagerLayer.Authorization;

public class RoleValidationAttribute : Attribute, IFilterFactory
{
    public string _roleName { get; set; }

    public RoleValidationAttribute(string roleName)
    {
        _roleName = roleName;
    }

    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var userRoleService = serviceProvider.GetRequiredService<IUserService>();
        return new RoleValidationFilter(_roleName, userRoleService);
    }
}
