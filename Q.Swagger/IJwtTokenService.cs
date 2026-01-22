using System.Security.Claims;

namespace Q.Swagger
{
    public interface IJwtTokenService
    {
        string BuildToken(IEnumerable<Claim> claims);
    }
}
