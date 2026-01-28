using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Q.Swagger.Jwt
{
    public class JwtTokenService(IOptions<JwtOptions> jwtOpt) : IJwtTokenService
    {
        private JwtOptions JwtOptions { get; init; } = jwtOpt.Value;

        public string BuildToken(IEnumerable<Claim> claims)
        {
            if (string.IsNullOrWhiteSpace(JwtOptions.SigningKey))
                throw new ApplicationException("JWT SigningKey not configured");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SigningKey));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                issuer: JwtOptions.Issuer,// 签发者
                audience: JwtOptions.Audience,// 受众
                claims: claims, //声明集合
                expires: DateTime.Now.AddSeconds(JwtOptions.ExpireSeconds),// 过期时间
                signingCredentials: creds);// 签名凭据
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
