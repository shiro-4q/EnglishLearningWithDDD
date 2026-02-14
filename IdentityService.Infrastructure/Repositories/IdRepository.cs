using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IdentityService.Infrastructure.Repositories
{
    public class IdRepository(IdUserManager userManager, RoleManager<Role> roleManager) : IIdRepository
    {
        public Task<User?> FindByIdAsync(Guid id)
        {
            return userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public Task<User?> FindByPhoneNumAsync(string phoneNum)
        {
            return userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNum);
        }

        public Task<User?> FindByUerNameAsync(string userName)
        {
            return userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return userManager.CreateAsync(user, password);
        }

        public Task<IdentityResult> AccessFailedAsync(User user)
        {
            return userManager.AccessFailedAsync(user);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new Role { Name = role });
                if (!result.Succeeded)
                    return result;
            }
            return await userManager.AddToRoleAsync(user, role);
        }

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            return userManager.CheckPasswordAsync(user, password);
        }

        public Task<bool> IsLockedOutAsync(User user)
        {
            return userManager.IsLockedOutAsync(user);
        }

        public async Task<IdentityResult> ChangePhoneNumAsync(Guid id, string phoneNum)
        {
            var user = await FindByIdAsync(id);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "用户没找到" });
            string token = await userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNum);
            return await userManager.ChangePhoneNumberAsync(user, phoneNum, token);
        }

        public async Task<(IdentityResult, User?, string? password)> ResetPasswordAsync(Guid id, string? password = null)
        {
            var user = await FindByIdAsync(id);
            if (user == null)
                return (IdentityResult.Failed(new IdentityError { Description = "用户没找到" }), null, null);
            if (string.IsNullOrWhiteSpace(password))
                password = GeneratePassword();
            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, password);
            if (!result.Succeeded)
                return (result, null, null);
            return (IdentityResult.Success, user, password);
        }

        public async Task<IdentityResult> ChangePasswordAsync(Guid id, string password)
        {
            if (password.Length < 6)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "Password Invalid",
                    Description = "密码长度不能少于6"
                });
            }
            var (result, _, _) = await ResetPasswordAsync(id, password);
            return result;
        }

        private string GeneratePassword()
        {
            var options = userManager.Options.Password;
            int length = options.RequiredLength;
            bool nonAlphanumeric = options.RequireNonAlphanumeric;
            bool digit = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;
            StringBuilder password = new StringBuilder();
            Random random = new Random();
            while (password.Length < length)
            {
                char c = (char)random.Next(32, 126);
                password.Append(c);
                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));
            return password.ToString();
        }
    }
}
