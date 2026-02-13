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
            var result = await CheckForLogin(user, password);
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
            var result = await CheckForLogin(user, password);
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

        #region 生成Token
        private async Task<string> BuildTokenByUser(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var roleList = await idRepository.GetRolesAsync(user);
            foreach (var role in roleList)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return jwtTokenService.BuildToken(claims);
        }
        #endregion

        #region 登录相关检查
        private async Task<SignInResult> CheckForLogin(User user, string password)
        {
            if (await idRepository.IsLockedOutAsync(user))
                return SignInResult.LockedOut;
            if (await idRepository.CheckPasswordAsync(user, password))
                return SignInResult.Success;
            else
            {
                var result = await idRepository.AccessFailedAsync(user);
                if (!result.Succeeded)
                    throw new ApplicationException("AccessFailed failed");
                return SignInResult.Failed;
            }
        }
        #endregion
    }
}
