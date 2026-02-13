using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Repositories
{
    public interface IIdRepository
    {
        Task<User?> FindByUerNameAsync(string userName);
        Task<User?> FindByPhoneNumAsync(string phoneNum);
        Task<User?> FindByIdAsync(Guid id);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<(IdentityResult, User?, string? password)> ResetPasswordAsync(Guid id, string? password = null);
        Task<IdentityResult> ChangePasswordAsync(Guid id, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> IsLockedOutAsync(User user);
        Task<IList<string>> GetRolesAsync(User user);
        Task<IdentityResult> AccessFailedAsync(User user);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
    }
}
