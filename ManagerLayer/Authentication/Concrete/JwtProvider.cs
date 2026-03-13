using EntityLayer.Entites.Users;
using EntityLayer.Options;
using ManagerLayer.Authentication.Abstract;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ManagerLayer.Authentication.Concrete;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public LoginResponse CreateToken(User user)
    {
        Claim[] claims =
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        DateTime expires = DateTime.UtcNow.AddHours(1);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256)
        );

        string securityToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResponse(user.Id, securityToken);
    }
}
