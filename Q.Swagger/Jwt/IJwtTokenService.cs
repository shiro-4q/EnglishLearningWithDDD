using System.Security.Claims;

namespace Q.Swagger.Jwt
{
    public interface IJwtTokenService
    {
        string BuildToken(IEnumerable<Claim> claims);
    }
}
