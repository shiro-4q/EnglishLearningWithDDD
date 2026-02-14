using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace IdentityService.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController(IIdRepository idRepository, IdDomainService idDomainService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Init()
        {
            var admin = await idRepository.FindByUerNameAsync("admin");
            if (admin != null)
                return StatusCode((int)HttpStatusCode.Conflict, "已经初始化过了");
            var user = new User("admin");
            var result = await idRepository.CreateUserAsync(user, "123456");
            Debug.Assert(result.Succeeded);
            result = await idRepository.ChangePhoneNumAsync(user.Id, "13711111111");
            Debug.Assert(result.Succeeded);
            result = await idRepository.AddToRoleAsync(user, "Admin");
            Debug.Assert(result.Succeeded);
            result = await idRepository.AddToRoleAsync(user, "User");
            Debug.Assert(result.Succeeded);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<string?>> LoginByPhoneNumAndPwd(LoginByPhoneNumAndPwdRequest request)
        {
            var (result, token) = await idDomainService.LoginByPhoneNumAndPwdAsync(request.PhoneNum, request.Password);
            if (result.Succeeded)
                return token;
            else if (result.IsLockedOut)
                return StatusCode((int)HttpStatusCode.Locked, "账号已被锁定");
            else
                return BadRequest("登录失败");
        }

        [HttpPost]
        public async Task<ActionResult<string?>> LoginByUserNameAndPwd(LoginByUserNameAndPwdRequest request)
        {
            var (result, token) = await idDomainService.LoginByUserNameAndPwdAsync(request.UserName, request.Password);
            if (result.Succeeded)
                return token;
            else if (result.IsLockedOut)
                return StatusCode((int)HttpStatusCode.Locked, "账号已被锁定");
            else
                return BadRequest("登录失败");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetUserInfo()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var user = await idRepository.FindByIdAsync(Guid.Parse(userId));
            if (user == null)
                return NotFound();
            return UserResponse.Create(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangeMyPassword(ChangeMyPasswordRequest request)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var (result, _, _) = await idRepository.ResetPasswordAsync(Guid.Parse(userId), request.Password);
            if (!result.Succeeded)
                return BadRequest("操作失败");
            return Ok();
        }
    }
}
