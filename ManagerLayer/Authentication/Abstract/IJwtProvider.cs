using EntityLayer.Entites.Users;
using ManagerLayer.Authentication.Concrete;

namespace ManagerLayer.Authentication.Abstract;

public interface IJwtProvider
{
    LoginResponse CreateToken(User user);
}
