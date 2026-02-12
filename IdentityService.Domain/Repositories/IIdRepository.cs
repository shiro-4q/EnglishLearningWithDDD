using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Repositories
{
    public interface IIdRepository
    {
        Task<User?> FindByUerNameAsync(string userName);
        Task<User?> FindByPhoneNumAsync(string phoneNum);
        Task<User?> FindByIdAsync(Guid id);
        Task<IdentityResult> CreateUserAsync(Guid id, string password);
        Task UpdatePasswordAsync(User user, string newPassword);
        Task<SignInResult> VerifyPasswordAsync(User user, string passwordHash);
        Task<IEnumerable<Role>> GetRolesAsync(User user);
        Task<IdentityResult> AccessFailedAsync(User user);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
    }
}
