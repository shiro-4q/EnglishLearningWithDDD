using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Q.Swagger.Jwt;
using System.Security.Claims;

namespace IdentityService.Domain.Services
{
    public class IdDomainService(IIdRepository idRepository, IJwtTokenService jwtTokenService)
    {
        public async Task<(SignInResult Result, string? Token)> LoginByPhoneNumAndPwdAsync(string phoneNum, string password)
        {
            var user = await idRepository.FindByPhoneNumAsync(phoneNum);
            if (user == null)
                return (SignInResult.Failed, null);
            var result = await idRepository.VerifyPasswordAsync(user, password);
            if (result.Succeeded)
            {
                var token = await BuildTokenByUser(user);
                return (result, token);
            }
            else
            {
                return (result, null);
            }
        }

        public async Task<(SignInResult Result, string? Token)> LoginByUserNameAndPwdAsync(string userName, string password)
        {
            var user = await idRepository.FindByUerNameAsync(userName);
            if (user == null)
                return (SignInResult.Failed, null);
            var result = await idRepository.VerifyPasswordAsync(user, password);
            if (result.Succeeded)
            {
                var token = await BuildTokenByUser(user);
                return (result, token);
            }
            else
            {
                return (result, null);
            }
        }

        public async Task<IdentityResult> ChangePasswordAsync(Guid userId, string password)
        {
            var user = await idRepository.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "用户不存在" });
            await idRepository.UpdatePasswordAsync(user, password);
            return IdentityResult.Success;
        }

        #region 生成Token
        private async Task<string> BuildTokenByUser(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var roleList = await idRepository.GetRolesAsync(user);
            roleList.ToList().ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name!));
            });
            return jwtTokenService.BuildToken(claims);
        }
        #endregion
    }
}
